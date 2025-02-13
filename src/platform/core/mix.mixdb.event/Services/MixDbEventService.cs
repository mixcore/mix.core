using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mix.Constant.Constants;
using Mix.Database.Constants;
using Mix.Database.Entities.MixDb;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Heart.Extensions;
using Mix.Heart.Services;
using Mix.Lib.Extensions;
using Mix.Mixdb.Event.ViewModels;
using Mix.Service.Commands;
using Mix.Service.Services;
using Mix.Shared.Models;
using Mix.Shared.Models.Configurations;
using Mix.Shared.Services;
using Mix.SignalR.Enums;
using Mix.SignalR.Interfaces;
using Mix.SignalR.Models;
using Newtonsoft.Json.Linq;

namespace Mix.Mixdb.Event.Services
{
    public class MixDbEventService
    {
        protected readonly IServiceProvider ServicesProvider;
        private readonly IConfiguration _configuration;
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
            ServicesProvider = servicesProvider;
            _configuration = configuration;
            PortalHub = portalHub;
            _databaseService = databaseService;
            _globalConfig = configuration.GetSection(MixAppSettingsSection.GlobalSettings).Get<GlobalSettingsModel>()!;
            LoadEvents();
            _httpService = httpService;
            _mixPermissionService = mixPermissionService;
        }

        public void LoadEvents(IServiceScope? serviceScope = null)
        {
            if (!_configuration.IsInit())
            {
                try
                {
                    var _serviceScope = serviceScope ?? ServicesProvider.CreateScope();
                    var mixDbDbContext = _serviceScope.ServiceProvider.GetRequiredService<MixDbDbContext>();
                    var repo = MixDbEventSubscriberViewModel.GetRootRepository(mixDbDbContext, null);
                    Subscribers = repo.GetAllAsync(m => !m.IsDeleted).GetAwaiter().GetResult();

                    if (serviceScope == null)
                    {
                        repo.UowInfo.Complete();
                        _serviceScope.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    MixLogService.LogExceptionAsync(ex);
                }
            }
        }

        public async Task HandleMessage(MixDbEventCommand model)
        {
            using (var serviceScope = ServicesProvider.CreateScope())
            {
                var _cacheService = serviceScope.ServiceProvider.GetRequiredService<MixCacheService>();
                if (model.Data != null)
                {
                    var id = model.Data.Id;
                    await _cacheService.RemoveCacheAsync(id, $"{MixFolders.MixDbCacheFolder}/{model.MixDbName}");
                }
                if (model.MixDbName == MixDatabaseNames.SYSTEM_PERMISSION || model.MixDbName == MixDatabaseNames.SYSTEM_PERMISSION_ENDPOINT)
                {
                    await _mixPermissionService.Reload();
                }
                else
                if (model.MixDbName == MixDbDatabaseNames.MixDbEvent || model.MixDbName == MixDbDatabaseNames.MixDbEventSubscriber)
                {
                    LoadEvents(serviceScope);
                }
                else
                {
                    var subs = Subscribers.Where(e => e.MixDbName == model.MixDbName
                                && e.Action!.Equals(model.Action, StringComparison.OrdinalIgnoreCase));
                    if (subs.Count() > 0)
                    {
                        foreach (var sub in subs)
                        {
                            if (sub.Callback != null && model.Data != null)
                            {
                                var requestModel = sub.Callback.ToObject<HttpRequestModel>();
                                requestModel!.Body = ParseBody(requestModel.Body, model.Data.After);
                                var result = await _httpService.SendHttpRequestModel(requestModel);
                                await SendMessage(model, requestModel!.Body, result);
                            }
                        }
                    }
                }
                serviceScope.Dispose();
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