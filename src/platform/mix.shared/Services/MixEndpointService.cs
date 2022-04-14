

namespace Mix.Shared.Services
{
    public class MixEndpointService : JsonConfigurationServiceBase
    {
        public MixEndpointService() : base(MixAppConfigFilePaths.Endpoint)
        {
        }

        public string Account
        {
            get => GetConfig<string>(MixModuleNames.Account);
            set => SetConfig(MixModuleNames.Account, value);
        }

        public string Common
        {
            get => GetConfig<string>(MixModuleNames.Common);
            set => SetConfig(MixModuleNames.Common, value);
        }

        public string Portal
        {
            get => GetConfig<string>(MixModuleNames.Portal);
            set => SetConfig(MixModuleNames.Portal, value);
        }

        public string Theme
        {
            get => GetConfig<string>(MixModuleNames.Theme);
            set => SetConfig(MixModuleNames.Theme, value);
        }

        public string Mixcore
        {
            get => GetConfig<string>(MixModuleNames.Mixcore);
            set => SetConfig(MixModuleNames.Mixcore, value);
        }

        public string Messenger
        {
            get => GetConfig<string>(MixModuleNames.Messenger);
            set => SetConfig(MixModuleNames.Messenger, value);
        }

        public string Scheduler
        {
            get => GetConfig<string>(MixModuleNames.Scheduler);
            set => SetConfig(MixModuleNames.Scheduler, value);
        }

        public string Grpc
        {
            get => GetConfig<string>(MixModuleNames.Grpc);
            set => SetConfig(MixModuleNames.Grpc, value);
        }
    }
}
