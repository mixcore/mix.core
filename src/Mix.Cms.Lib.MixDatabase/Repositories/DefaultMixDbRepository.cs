using System;
using System.Data.Common;

namespace Mix.Cms.Lib.MixDatabase.Repositories
{
    public class DefaultMixDbRepository<TDbConnection, TEntity> : RepositoryBase<TDbConnection, TEntity>
    where TDbConnection : DbConnection
    where TEntity : class
    {
        /// <summary>
        /// The instance
        /// </summary>
        private static volatile DefaultMixDbRepository<TDbConnection, TEntity> instance;

        /// <summary>
        /// The synchronize root
        /// </summary>
        private static readonly object syncRoot = new Object();

        /// <summary>
        /// Prevents a default instance of the <see cref="DefaultMixDbRepository{TDbContext, TModel, TView}"/> class from being created.
        /// </summary>
        public DefaultMixDbRepository() : base()
        {
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static DefaultMixDbRepository<TDbConnection, TEntity> Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new DefaultMixDbRepository<TDbConnection, TEntity>();
                    }
                }

                return instance;
            }
        }
    }
}
