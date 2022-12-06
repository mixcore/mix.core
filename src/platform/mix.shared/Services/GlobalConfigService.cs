using Mix.Shared.Models.Configurations;

namespace Mix.Shared.Services
{
    public class GlobalConfigService : ConfigurationServiceBase<GlobalConfigurations>
    {

        #region Instance
        /// <summary>
        /// The synchronize root
        /// </summary>
        protected static readonly object SyncRoot = new object();

        /// <summary>
        /// The instance
        /// </summary>
        private static GlobalConfigService _instance;

        public static GlobalConfigService Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new();
                        }
                    }
                }

                return _instance;
            }
        }
        #endregion

        public GlobalConfigService()
            : base(MixAppConfigFilePaths.Global)
        {
        }
        public new string AesKey
        {
            get => Instance.AppSettings.ApiEncryptKey;
            set => Instance.AppSettings.ApiEncryptKey = value;
        }
        public int ResponseCache => AppSettings.ResponseCache;
        public bool IsInit => AppSettings.IsInit;
        public bool AllowAnyOrigin => AppSettings.AllowAnyOrigin;
        public InitStep InitStatus => AppSettings.InitStatus;
    }
}
