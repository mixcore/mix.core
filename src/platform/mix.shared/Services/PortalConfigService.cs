

namespace Mix.Shared.Services
{
    public class PortalConfigService : JsonConfigurationServiceBase
    {
        #region Instance
        /// <summary>
        /// The synchronize root
        /// </summary>
        protected static readonly object syncRoot = new object();

        /// <summary>
        /// The instance
        /// </summary>
        private static PortalConfigService instance;

        public static PortalConfigService Instance
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

        public PortalConfigService()
            : base(MixAppConfigFilePaths.Portal)
        {
        }
    }
}
