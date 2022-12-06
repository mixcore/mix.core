

namespace Mix.Shared.Services
{
    public class PortalConfigService : JsonConfigurationServiceBase
    {
        #region Instance
        /// <summary>
        /// The synchronize root
        /// </summary>
        protected static readonly object SyncRoot = new object();

        /// <summary>
        /// The instance
        /// </summary>
        private static PortalConfigService _instance;

        public static PortalConfigService Instance
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

        public PortalConfigService()
            : base(MixAppConfigFilePaths.Portal)
        {
        }
    }
}
