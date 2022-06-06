

namespace Mix.Shared.Services
{
    public class MixEndpointService : JsonConfigurationServiceBase
    {
        #region Instance
        /// <summary>
        /// The synchronize root
        /// </summary>
        protected static readonly object syncRoot = new object();

        /// <summary>
        /// The instance
        /// </summary>
        private static MixEndpointService instance;

        public static MixEndpointService Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        public MixEndpointService() : base(MixAppConfigFilePaths.Endpoint)
        {
        }

        protected override void LoadAppSettings()
        {
            base.LoadAppSettings();
            Endpoints = AppSettings.Properties().Select(m => m.Value.ToString()).ToArray();
        }

        public string[] Endpoints;
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
