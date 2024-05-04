﻿using Mix.Database.Services;

namespace Mix.Database.Entities.QueueLog
{
    public class QueueLogDbContext : BaseDbContext
    {
        public QueueLogDbContext(DatabaseService databaseService) : base(databaseService, MixConstants.CONST_QUEUE_LOG_CONNECTION)
        {
        }

        public QueueLogDbContext(DatabaseService databaseService, string connectionStringName) : base(databaseService, connectionStringName)
        {
        }

        public QueueLogDbContext(string connectionString, MixDatabaseProvider databaseProvider) : base(connectionString, databaseProvider)
        {
        }

        public virtual DbSet<QueueLog> QueueLog { get; set; }

    }
}
