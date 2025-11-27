using FirebaseAdmin.Auth.Multitenancy;
using Microsoft.CodeAnalysis.CodeActions;
using Mix.Automation.Lib.Entities;
using Mix.Automation.Lib.Enums;
using Mix.Automation.Lib.Models;
using Mix.Automation.Lib.ViewModels;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Heart.Extensions;
using Mix.Heart.Helpers;
using Mix.Heart.Services;
using Mix.Heart.UnitOfWork;
using Mix.Mq.Lib.Models;
using Mix.Queue.Interfaces;
using Mix.Shared.Models;
using Mix.Shared.Services;
using Mix.SignalR.Hubs;
using Mix.SignalR.Interfaces;
using Mix.SignalR.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Mix.Automation.Lib.Services
{
    public sealed class WorkflowService
    {
        protected readonly IMemoryQueueService<MessageQueueModel> _queueService;
        private readonly IPortalHubClientService _portalHub;
        private DatabaseService _databaseService;
        private MixCacheService _cacheService;
        private readonly HttpService _httpService;
        private UnitOfWorkInfo<WorkflowDbContext> _uow;

        public WorkflowService(UnitOfWorkInfo<WorkflowDbContext> uow, MixCacheService cacheService, DatabaseService databaseService, HttpService httpService, IPortalHubClientService portalHub, IMemoryQueueService<MessageQueueModel> queueService)
        {
            _uow = uow;
            _cacheService = cacheService;
            _databaseService = databaseService;
            _httpService = httpService;
            _portalHub = portalHub;
            _queueService = queueService;
        }

        public async Task CreateTrigger(CreateWorkflowTriggerModel dto, CancellationToken cancellationToken = default)
        {
            var wf = await WorkflowViewModel.GetRepository(_uow, _cacheService).GetSingleAsync(m => m.Id == dto.WorkflowId);
            if (wf != null)
            {
                var trigger = new WorkflowTriggerViewModel()
                {
                    WorkflowId = dto.WorkflowId,
                    Input = dto.Input,
                    Actions = new List<WorkflowActionDataViewModel>()
                };

                JArray responses = new JArray();
                if (wf != null)
                {
                    foreach (var action in wf.Actions.OrderBy(m => m.Index))
                    {
                        trigger.Actions.Add(new WorkflowActionDataViewModel()
                        {
                            ActionId = action.Id,
                            ActionType = action.Type,
                            Index = action.Index,
                            Request = action.Request,
                            Body = action.Body
                        });
                    }
                    trigger.SetUowInfo(_uow, _cacheService);
                    await trigger.SaveAsync(cancellationToken);
                    await _uow.CompleteAsync(cancellationToken);


                    var msg = new MessageQueueModel(1)
                    {
                        Success = true,
                        TopicId = MixAutomationConstants.Topics.MixAutomation,
                        Action = MixAutomationAction.ExecuteTrigger.ToString(),
                        Data = ReflectionHelper.ParseObject(new ExecuteWorkflowTriggerModel()
                        {
                            TriggerId = trigger.Id
                        }).ToString(Formatting.None)
                    };

                    _queueService.PushMemoryQueue(msg);
                }
            }
        }

        public async Task TriggerWorkflow(ExecuteWorkflowTriggerModel dto, CancellationToken cancellationToken = default)
        {
            var trigger = await WorkflowTriggerViewModel.GetRepository(_uow, _cacheService).GetSingleAsync(m => m.Id == dto.TriggerId);
            if (trigger != null)
            {
                JArray responses = new JArray();
                foreach (var action in trigger.Actions.OrderBy(m => m.Index))
                {
                    if (action.Index == 0)
                    {
                        if (trigger.Input != null)
                        {
                            action.Request = InjectParameters(action.Request, trigger.Input);
                            action.Body = InjectParameters(action.Body, trigger.Input);
                        }
                    }
                    else
                    {
                        action.Request = InjectParameters(action.Request, responses);
                        action.Body = InjectParameters(action.Body, responses);
                    }
                    await ExecuteAction(action, responses, cancellationToken);
                    responses.Add(action.Response ?? new JObject());
                    if (!action.IsSuccess)
                    {
                        break;
                    }
                }
                trigger.SetUowInfo(_uow, _cacheService);
                await trigger.SaveAsync(cancellationToken);
                await _uow.CompleteAsync(cancellationToken);
            }
        }

        private async Task ExecuteAction(WorkflowActionDataViewModel action, JArray responses, CancellationToken cancellationToken)
        {
            JObject? resp = default;
            DateTime start = DateTime.UtcNow;
            action.ActionStatus = ActionStatus.Started;
            action.CreatedDateTime = DateTime.UtcNow;
            action.LastModified = DateTime.UtcNow;
            try
            {
                await NotifyWorkflowActionStatus(action, cancellationToken);
                switch (action.ActionType)
                {
                    case ActionType.Request:
                        if (action.Request != null)
                        {
                            var request = action.Request.ToObject<HttpRequestModel>();
                            if (request == null || string.IsNullOrEmpty(request.RequestUrl))
                            {
                                action.IsSuccess = false;
                                action.ActionStatus = ActionStatus.Failed;
                                action.Response = new JObject(new JProperty("error", "RequestUrl cannot be nul"));
                                break;
                            }

                            request!.Body = action.Body;
                            action.Request = action.Request;
                            action.Body = action.Body;
                            action.Response = await _httpService.SendHttpRequestModel(request);
                        }
                        break;
                    case ActionType.ParseMarkdownJson:
                        string markdownJsonPattern = "\\`{3}(\\w+)?\\n([^\\`]+)\\n\\`{3}";
                        action.Body = InjectParameters(action.Body, responses);
                        var markdownContent = action.Body!.Value<string>("content");
                        if (!string.IsNullOrEmpty(markdownContent) && Regex.IsMatch(markdownContent, markdownJsonPattern))
                        {
                            action.Response = JObject.Parse(Regex.Match(markdownContent, markdownJsonPattern).Groups[2].Value.Replace("\\\"", "\""));
                        }
                        break;
                    case ActionType.PortalNotification:
                        var portalMsg = InjectParameters(action.Body, responses);
                        action.Body = new JObject(new JProperty("input", portalMsg));
                        await _portalHub.SendMessageAsync(portalMsg.ToObject<SignalRMessageModel>());
                        action.Response = new JObject();
                        break;
                    default:
                        break;
                }

                action.IsSuccess = true;
                action.ActionStatus = ActionStatus.Succeeded;
                action.Duration = (DateTime.UtcNow - start).TotalMilliseconds;
                await NotifyWorkflowActionStatus(action, cancellationToken);
            }
            catch (Exception ex)
            {
                action.IsSuccess = false;
                action.Exception = JObject.FromObject(ex);
                action.ActionStatus = ActionStatus.Failed;
                action.Duration = (DateTime.UtcNow - start).TotalMilliseconds;
                await NotifyWorkflowActionStatus(action, cancellationToken);
            }
        }

        private async Task NotifyWorkflowActionStatus(WorkflowActionDataViewModel action, CancellationToken cancellationToken)
        {
            await _portalHub.SendMessageAsync(new SignalRMessageModel()
            {
                Action = SignalR.Enums.MessageAction.NewMessage,
                Data = action,
                Title = "Action Status",
                Message = action.ActionStatus.ToString()
            });
        }

        #region Private Methods

        private JObject? InjectParameters(JObject? body, JToken responses)
        {
            string pattern = "(\\[{2})((\\d|\\.|\\w)+)(\\]{2})";
            string? strBody = body?.ToString(Formatting.None);
            if (string.IsNullOrEmpty(strBody) || !Regex.IsMatch(strBody, pattern))
            {
                return body;
            }
            else
            {
                foreach (Match match in Regex.Matches(strBody, pattern))
                {
                    strBody = strBody.Replace(match.Value, ExtractInput<string>(match.Groups[2].Value, responses));
                }
                return JObject.Parse(strBody);
            }
        }

        private T? ExtractInput<T>(string inputPath, JToken jObject)
        {
            if (jObject is null)
            {
                return default;
            }

            if (string.IsNullOrEmpty(inputPath))
            {
                return jObject.ToObject<T>();
            }

            var keys = inputPath.Split('.');
            JToken? token = jObject;
            foreach (var key in keys)
            {
                if (int.TryParse(key, out var index))
                {
                    if (token.Type == JTokenType.Array)
                    {
                        token = JArray.FromObject(token)[index];
                    }
                    else
                    {
                        token = token[key];
                    }
                }
                else
                {
                    token = token[key];
                }
            }
            return token != null ? token.ToObject<T>() : default;
        }

        #endregion
    }
}
