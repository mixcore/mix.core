using Mix.Shared.Models.Configurations;

namespace Mix.Shared.Services
{
    public class GlobalConfigService : ConfigurationServiceBase<GlobalConfigurations>
    {

        #region Instance
        /// <summary>
        /// The synchronize root
        /// </summary>
        protected static readonly object syncRoot = new object();

        /// <summary>
        /// The instance
        /// </summary>
        private static GlobalConfigService instance;

        public static GlobalConfigService Instance
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

        public GlobalConfigService()
            : base(MixAppConfigFilePaths.Global)
        {
        }
        public new string AesKey
        {
            get { return Instance.AppSettings.ApiEncryptKey; }
            set { Instance.AppSettings.ApiEncryptKey = value; }
        }
        public int ResponseCache => AppSettings.ResponseCache;
        public bool IsInit => AppSettings.IsInit;
        public InitStep InitStatus => AppSettings.InitStatus;
    }
}
