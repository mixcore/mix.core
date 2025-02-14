using Microsoft.Extensions.Configuration;
using Mix.Constant.Constants;
using Mix.Database.Entities.Settings;
using System.Linq;

namespace Mix.Database.Services.MixGlobalSettings
{
    public sealed class MixEndpointService : GlobalSettingServiceBase
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

        protected override void LoadAppSettings()
        {
            base.LoadAppSettings();
            Endpoints = RawSettings.Properties().Select(m => m.Value.ToString()).ToArray();
        }

        public string[] Endpoints;

        public string GetEndpoint(string name)
        {
            return RawSettings?.Value<string>(name);
        }

        public string DefaultDomain
        {
            get => GetConfig<string>(MixModuleNames.Default);
            set => SetConfig(MixModuleNames.Default, value);
        }
        public string MixMq
        {
            get => GetConfig<string>(MixModuleNames.MixMq) ?? DefaultDomain;
            set => SetConfig(MixModuleNames.MixMq, value);
        }
        public string Account
        {
            get => GetConfig<string>(MixModuleNames.Account) ?? DefaultDomain;
            set => SetConfig(MixModuleNames.Account, value);
        }

        public string Common
        {
            get => GetConfig<string>(MixModuleNames.Common) ?? DefaultDomain;
            set => SetConfig(MixModuleNames.Common, value);
        }

        public string Portal
        {
            get => GetConfig<string>(MixModuleNames.Portal) ?? DefaultDomain;
            set => SetConfig(MixModuleNames.Portal, value);
        }

        public string Theme
        {
            get => GetConfig<string>(MixModuleNames.Theme) ?? DefaultDomain;
            set => SetConfig(MixModuleNames.Theme, value);
        }

        public string Mixcore
        {
            get => GetConfig<string>(MixModuleNames.Mixcore) ?? DefaultDomain;
            set => SetConfig(MixModuleNames.Mixcore, value);
        }

        public string Messenger
        {
            get => GetConfig<string>(MixModuleNames.Messenger) ?? DefaultDomain;
            set => SetConfig(MixModuleNames.Messenger, value);
        }

        public string Scheduler
        {
            get => GetConfig<string>(MixModuleNames.Scheduler) ?? DefaultDomain;
            set => SetConfig(MixModuleNames.Scheduler, value);
        }

        public string Grpc
        {
            get => GetConfig<string>(MixModuleNames.Grpc) ?? DefaultDomain;
            set => SetConfig(MixModuleNames.Grpc, value);
        }
    }
}
