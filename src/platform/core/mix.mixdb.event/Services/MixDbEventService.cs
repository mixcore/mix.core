using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Amqp.Framing;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Mix.Database.Services;
using Mix.Heart.Extensions;
using Mix.Heart.Services;
using Mix.Heart.UnitOfWork;
using Mix.Mixdb.Entities;
using Mix.Mixdb.Event.ViewModels;
using Mix.Queue.Models;
using Mix.RepoDb.Services;
using Mix.Service.Commands;
using Mix.Service.Services;
using Mix.Shared.Models;
using Mix.Shared.Services;
using Mix.SignalR.Enums;
using Mix.SignalR.Interfaces;
using Mix.SignalR.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Mix.Mixdb.Event.Services
{
    public class MixDbEventService
    {
        private readonly IPortalHubClientService PortalHub;
        private DatabaseService _databaseService;
        public List<MixDbEventSubscriberViewModel> Subscribers;
        private readonly HttpService _httpService;
        public MixDbEventService(DatabaseService databaseService, HttpService httpService, IPortalHubClientService portalHub)
        {
            _databaseService = databaseService;
            LoadEvents();
            _httpService = httpService;
            PortalHub = portalHub;
        }

        public void LoadEvents()
        {
            if (!GlobalConfigService.Instance.IsInit)
            {
                var repo = MixDbEventSubscriberViewModel.GetRootRepository(new MixDbDbContext(_databaseService), null);
                Subscribers = repo.GetAllAsync(m => !m.IsDeleted).GetAwaiter().GetResult();
                repo.UowInfo.Complete();
            }
        }

        public async Task HandleMessage(MixDbEventCommand model)
        {
            if (model.MixDbName == MixDbDatabaseNames.DatabaseNameMixDbEvent || model.MixDbName == MixDbDatabaseNames.DatabaseNameMixDbEventSubscriber)
            {
                LoadEvents();
            }
            else
            {
                var subs = Subscribers.Where(e => e.MixDbName == model.MixDbName
                            && e.Action!.Equals(model.Action, StringComparison.OrdinalIgnoreCase));
                if (subs.Count() > 0)
                {
                    foreach (var sub in subs)
                    {
                        if (sub.Callback != null)
                        {
                            var requestModel = sub.Callback.ToObject<HttpRequestModel>();
                            requestModel!.Body = ParseBody(requestModel.Body, model.Data);
                            var result = await _httpService.SendHttpRequestModel(requestModel);
                            await SendMessage(model, requestModel!.Body, result);
                        }
                    }
                }
            }
        }

        private JObject ParseBody(JObject body, JObject data)
        {

            if (body == null)
            {
                return data;
            }
            else
            {
                string strBody = body.ToString(Newtonsoft.Json.Formatting.None);
                foreach (var prop in data.Properties().ToList())
                {
                    if (strBody.Contains($"[[{prop.Name}]]", StringComparison.OrdinalIgnoreCase))
                    {
                        strBody = strBody.Replace($"[[{prop.Name.ToTitleCase()}]]", data.GetValue(prop.Name).ToString());
                    }
                }
                return JObject.Parse(strBody);
            }
        }

        private async Task SendMessage(MixDbEventCommand cmd, JObject body, JObject? result)
        {
            SignalRMessageModel msg = new()
            {
                Action = MessageAction.NewMessage,
                Type = MessageType.Success,
                Title = $"{cmd.MixDbName} - {cmd.Action}",
                Data = body?.ToString(Newtonsoft.Json.Formatting.None),
                From = new(GetType().FullName),
                Message = result?.ToString()
            };
            await PortalHub.SendMessageAsync(msg);
        }
    }
}
