namespace Mix.Shared.Services
{
    public sealed class MixEndpointService : JsonConfigurationServiceBase
    {
        private string _defaultDomain;
        public MixEndpointService() : base(MixAppConfigFilePaths.Endpoint)
        {
        }

        public void SetDefaultDomain(string domain)
        {
            _defaultDomain = domain;
        }

        protected override void LoadAppSettings()
        {
            base.LoadAppSettings();
            Endpoints = AppSettings.Properties().Select(m => m.Value.ToString()).ToArray();
        }

        public string[] Endpoints;
        public string Account
        {
            get => GetConfig<string>(MixModuleNames.Account) ?? _defaultDomain;
            set => SetConfig(MixModuleNames.Account, value);
        }

        public string Common
        {
            get => GetConfig<string>(MixModuleNames.Common) ?? _defaultDomain;
            set => SetConfig(MixModuleNames.Common, value);
        }

        public string Portal
        {
            get => GetConfig<string>(MixModuleNames.Portal) ?? _defaultDomain;
            set => SetConfig(MixModuleNames.Portal, value);
        }

        public string Theme
        {
            get => GetConfig<string>(MixModuleNames.Theme) ?? _defaultDomain;
            set => SetConfig(MixModuleNames.Theme, value);
        }

        public string Mixcore
        {
            get => GetConfig<string>(MixModuleNames.Mixcore) ?? _defaultDomain;
            set => SetConfig(MixModuleNames.Mixcore, value);
        }

        public string Messenger
        {
            get => GetConfig<string>(MixModuleNames.Messenger) ?? _defaultDomain;
            set => SetConfig(MixModuleNames.Messenger, value);
        }

        public string Scheduler
        {
            get => GetConfig<string>(MixModuleNames.Scheduler) ?? _defaultDomain;
            set => SetConfig(MixModuleNames.Scheduler, value);
        }

        public string Grpc
        {
            get => GetConfig<string>(MixModuleNames.Grpc) ?? _defaultDomain;
            set => SetConfig(MixModuleNames.Grpc, value);
        }
    }
}
