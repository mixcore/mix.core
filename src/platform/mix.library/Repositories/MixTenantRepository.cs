namespace Mix.Lib.Repositories
{
    public class MixTenantRepository
    {
        #region Instance
        /// <summary>
        /// The synchronize root
        /// </summary>
        protected static readonly object syncRoot = new object();

        /// <summary>
        /// The instance
        /// </summary>
        private static MixTenantRepository instance;

        public static MixTenantRepository Instance
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
        public List<MixTenant> AllTenants { get; set; }

        public MixTenant GetCurrentTenant(string host)
        {
            return AllTenants.FirstOrDefault(m => m.PrimaryDomain == host);
        }
    }
}
