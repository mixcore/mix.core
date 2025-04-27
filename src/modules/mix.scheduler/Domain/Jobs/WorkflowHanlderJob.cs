using Mix.Queue.Interfaces;
using Mix.SignalR.Models;
using Newtonsoft.Json.Linq;
using Quartz;
using System;
using System.Threading.Tasks;
using Mix.Quartz.Jobs;
using Mix.SignalR.Interfaces;
using Mix.Mq.Lib.Models;
using Mix.Shared.Models;
using Mix.Shared.Services;
using System.Collections.Generic;
using System.Linq;
using Mix.Mixdb.Services;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Mixdb.Interfaces;
using Mix.Heart.Models;
using Microsoft.Identity.Client;
using Mix.Heart.Extensions;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Routing.Matching;

namespace Mix.Scheduler.Domain.Jobs
{
    public class WorkflowHanlderJob : MixJobBase
    {
        private readonly HttpService _httpService;
        private readonly IPortalHubClientService _portalHub;
        protected readonly IMixDbDataService _mixDbDataService;
        public WorkflowHanlderJob(
            IServiceProvider serviceProvider,
            IMemoryQueueService<MessageQueueModel> queueService,
            IPortalHubClientService portalHub,
            HttpService httpService,
            DatabaseService databaseService,
            IMixDbDataServiceFactory mixDbDataFactory)
            : base(serviceProvider, queueService)
        {
            _portalHub = portalHub;
            _httpService = httpService;
            _mixDbDataService = mixDbDataFactory.Create(databaseService.DatabaseProvider, databaseService.GetConnectionString(MixConstants.CONST_MIXDB_CONNECTION));
        }

        public override async Task ExceptionHandler(Exception ex)
        {
            await _portalHub.SendMessageAsync(new SignalRMessageModel()
            {
                Action = SignalR.Enums.MessageAction.NewMessage,
                Type = SignalR.Enums.MessageType.Error,
                From = new HubUserModel(nameof(WorkflowTriggerModel)),
                Title = "Exception",
                Message = ex.Message
            });
            await base.ExceptionHandler(ex);
        }

        public override async Task ExecuteHandler(IJobExecutionContext context)
        {
            var obj = context.Trigger.JobDataMap.GetString("data");
            if (!string.IsNullOrWhiteSpace(obj))
            {
                var msg = JObject.Parse(obj).ToObject<WorkflowTriggerModel>();
                var wf = await _mixDbDataService.GetByIdAsync<WorkflowModel>("mix_workflow", msg.WorkflowId,
                    "title,actions", System.Threading.CancellationToken.None);
                JArray responses = new JArray();
                if (wf != null)
                {
                    foreach (var action in wf.Actions.Items.OrderBy(m => m.Index))
                    {
                        if (action.Index == 0)
                        {
                            if (msg.Input != null)
                            {
                                action.Request = InjectParameters(action.Request, msg.Input);
                                action.Body = InjectParameters(action.Body, msg.Input);
                            }
                        }
                        else
                        {
                            action.Request = InjectParameters(action.Request, responses);
                            action.Body = InjectParameters(action.Body, responses);
                        }
                        if (action.Type == "Request")
                        {
                            var request = action.Request.ToObject<HttpRequestModel>();
                            request.Body = action.Body;
                            var resp = await _httpService.SendHttpRequestModel(request);
                            responses.Add(resp);
                        }
                        if (action.Type == "ParseMarkdownJson")
                        {
                            var markdownContent = ExtractInput<string>(action.InputPath, responses);
                            responses.Add(JObject.Parse(Regex.Match(markdownContent, "\\`{3}(\\w+)?\\n([^\\`]+)\\n\\`{3}").Groups[2].Value));
                        }
                        if (action.Type == "PortalNotification")
                        {
                            var portalMsg = InjectParameters(action.Body, responses);
                            await _portalHub.SendMessageAsync(portalMsg.ToObject<SignalRMessageModel>());
                        }
                    }
                }
            }
        }


        // pattern: {responseIndex}.{propertyName}
        private JObject InjectParameters(JObject body, JToken responses)
        {
            string pattern = "(\\[{2})((\\d|\\.|\\w)+)(\\]{2})";
            string strBody = body.ToString(Formatting.None);
            if (!Regex.IsMatch(strBody, pattern))
            {
                return body;
            }
            else
            {
                foreach (Match match in Regex.Matches(strBody, pattern))
                {
                    strBody = strBody.Replace(match.Value, @ExtractInput<string>(match.Groups[2].Value, responses));
                }
                return JObject.Parse(strBody);
            }
        }

        private T ExtractInput<T>(string inputPath, JToken jObject)
        {
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

        public class WorkflowTriggerModel
        {
            public int WorkflowId { get; set; }
            public JObject? Input { get; set; }
        }
        public class WorkflowModel
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public PagingResponseModel<WorkflowActionModel> Actions { get; set; }
        }
        public class WorkflowActionModel
        {
            [JsonProperty("index")]
            public int Index { get; set; }
            [JsonProperty("title")]
            public string Title { get; set; }
            [JsonProperty("type")]
            public string Type { get; set; }
            [JsonProperty("request")]
            public JObject? Request { get; set; }
            [JsonProperty("input_path")]
            public string? InputPath { get; set; } // from ancestor action
            [JsonProperty("body")]
            public JObject? Body { get; set; } // current action
        }
        public class ChatCompletion
        {
            public string Id { get; set; }
            public string Object { get; set; }
            public long Created { get; set; }
            public string Model { get; set; }
            public List<Choice> Choices { get; set; }
            public Usage Usage { get; set; }
        }

        public class Choice
        {
            public int Index { get; set; }
            public Message Message { get; set; }
            public object Logprobs { get; set; } // Use object if you’re unsure of the exact type
            public string FinishReason { get; set; }
        }

        public class Message
        {
            public string Role { get; set; }
            public string Content { get; set; }
        }

        public class Usage
        {
            public int PromptTokens { get; set; }
            public int CompletionTokens { get; set; }
            public int TotalTokens { get; set; }
            public PromptTokensDetails PromptTokensDetails { get; set; }
        }

        public class PromptTokensDetails
        {
            public int CachedTokens { get; set; }
        }
    }


}