using Microsoft.Extensions.Configuration;
using Mix.Constant.Constants;
using Mix.Database.Entities.Settings;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace Mix.Database.Services.MixGlobalSettings
{
    public sealed class MixEndpointService : GlobalSettingServiceBase<JObject>
    {
        public MixEndpointService(IConfiguration configuration, MixGlobalSetting settings) : base(configuration, settings)
        {
        }

        public void SetDefaultDomain(string domain)
        {
            Messenger ??= domain;
            MixMq ??= domain;
            Mixcore ??= domain;
        }
        
        public void InitDomain(string domain)
        {
            DefaultBaseUrl ??= domain;
            Account ??= domain;
            Mixcore ??= domain;
            Messenger ??= domain;
            MixMq ??= domain;
            SaveSettings();
        }

        protected override void LoadAppSettings()
        {
            base.LoadAppSettings();
            Endpoints = AppSettings.Properties().Select(m => m.Value.ToString()).ToArray();
        }

        public string[] Endpoints;

        public string GetEndpoint(string name)
        {
            return AppSettings?.Value<string>(name);
        }

        public string DefaultBaseUrl
        {
            get => GetConfig<string>(MixModuleNames.Default);
            set => SetConfig(MixModuleNames.Default, value);
        }
        public string MixMq
        {
            get => GetConfig<string>(MixModuleNames.MixMq) ?? DefaultBaseUrl;
            set => SetConfig(MixModuleNames.MixMq, value);
        }
        public string Account
        {
            get => GetConfig<string>(MixModuleNames.Account) ?? DefaultBaseUrl;
            set => SetConfig(MixModuleNames.Account, value);
        }

        public string Common
        {
            get => GetConfig<string>(MixModuleNames.Common) ?? DefaultBaseUrl;
            set => SetConfig(MixModuleNames.Common, value);
        }

        public string Portal
        {
            get => GetConfig<string>(MixModuleNames.Portal) ?? DefaultBaseUrl;
            set => SetConfig(MixModuleNames.Portal, value);
        }

        public string Theme
        {
            get => GetConfig<string>(MixModuleNames.Theme) ?? DefaultBaseUrl;
            set => SetConfig(MixModuleNames.Theme, value);
        }

        public string Mixcore
        {
            get => GetConfig<string>(MixModuleNames.Mixcore) ?? DefaultBaseUrl;
            set => SetConfig(MixModuleNames.Mixcore, value);
        }

        public string Messenger
        {
            get => GetConfig<string>(MixModuleNames.Messenger) ?? DefaultBaseUrl;
            set => SetConfig(MixModuleNames.Messenger, value);
        }

        public string Scheduler
        {
            get => GetConfig<string>(MixModuleNames.Scheduler) ?? DefaultBaseUrl;
            set => SetConfig(MixModuleNames.Scheduler, value);
        }

        public string Grpc
        {
            get => GetConfig<string>(MixModuleNames.Grpc) ?? DefaultBaseUrl;
            set => SetConfig(MixModuleNames.Grpc, value);
        }
    }
}
