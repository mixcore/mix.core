using Mix.Shared.Constants;
using Mix.Shared.Enums;
using Mix.Shared.Models;

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

        public bool IsInit => AppSettings.IsInit;
        public string DefaultCulture => AppSettings.DefaultCulture;
        public string Domain => AppSettings.Domain;
        public InitStep InitStatus => AppSettings.InitStatus;
    }
}
