using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Amqp.Framing;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mix.Constant.Constants;
using Mix.Database.Constants;
using Mix.Database.Entities.MixDb;
using Mix.Database.Services;
using Mix.Heart.Extensions;
using Mix.Heart.Services;
using Mix.Heart.UnitOfWork;

using Mix.Mixdb.Event.ViewModels;
using Mix.Queue.Models;
using Mix.RepoDb.Services;
using Mix.Service.Commands;
using Mix.Service.Services;
using Mix.Shared.Models;
using Mix.Shared.Models.Configurations;
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
        protected readonly IServiceProvider ServicesProvider;
        private readonly IPortalHubClientService PortalHub;
        private DatabaseService _databaseService;
        private MixPermissionService _mixPermissionService;
        public List<MixDbEventSubscriberViewModel> Subscribers;
        private readonly GlobalSettingsModel _globalConfig;
        private readonly HttpService _httpService;
        public MixDbEventService(DatabaseService databaseService, HttpService httpService,
            IPortalHubClientService portalHub,
            MixPermissionService mixPermissionService,
            IServiceProvider servicesProvider,
            IConfiguration configuration)
        {
            _databaseService = databaseService;
            _globalConfig = configuration.GetSection(MixAppSettingsSection.GlobalSettings).Get<GlobalSettingsModel>()!;
            LoadEvents();
            _httpService = httpService;
            PortalHub = portalHub;
            _mixPermissionService = mixPermissionService;
            ServicesProvider = servicesProvider;
        }

        public void LoadEvents()
        {
            if (!_globalConfig.IsInit)
            {
                var repo = MixDbEventSubscriberViewModel.GetRootRepository(new MixDbDbContext(_databaseService), null);
                Subscribers = repo.GetAllAsync(m => !m.IsDeleted).GetAwaiter().GetResult();
                repo.UowInfo.Complete();
            }
        }

        public async Task HandleMessage(MixDbEventCommand model)
        {
            using (var serviceScope = ServicesProvider.CreateScope())
            {
                var _cacheService = serviceScope.ServiceProvider.GetRequiredService<MixCacheService>();
                await _cacheService.RemoveCacheAsync(model.Data.Value<string>("Id"), $"{MixFolders.MixDbCacheFolder}/{model.MixDbName}");
                if (model.MixDbName == MixDatabaseNames.SYSTEM_PERMISSION || model.MixDbName == MixDatabaseNames.SYSTEM_PERMISSION_ENDPOINT)
                {
                    await _mixPermissionService.Reload();
                }
                else
                if (model.MixDbName == MixDbDatabaseNames.MixDbEvent || model.MixDbName == MixDbDatabaseNames.MixDbEventSubscriber)
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
                        strBody = strBody.Replace($"[[{prop.Name.ToTitleCase()}]]", data.GetValue(prop.Name)!.ToString());
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