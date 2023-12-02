namespace Mix.Shared.Services
{
    public sealed class MixEndpointService : JsonConfigurationServiceBase
    {
        public string DefaultDomain;
        public MixEndpointService() : base(MixAppConfigFilePaths.Endpoint)
        {
        }

        public void SetDefaultDomain(string domain)
        {
            DefaultDomain = domain;
        }

        protected override void LoadAppSettings()
        {
            base.LoadAppSettings();
            Endpoints = AppSettings.Properties().Select(m => m.Value.ToString()).ToArray();
        }

        public string[] Endpoints;
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
