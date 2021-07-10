using Microsoft.EntityFrameworkCore;
using Mix.Heart.Enums;
using System;
using System.Reflection;

namespace Mix.Xunittest.Domain.Base
{
    // Ref: https://docs.microsoft.com/en-us/ef/core/testing/sharing-databases
    public abstract class SharedDatabaseFixture<TDbContext> 
        where TDbContext : DbContext
    {
        private static readonly object _lock = new object();
        private static bool _databaseInitialized;
        protected static ConstructorInfo ctor;

        protected readonly string _connectionString = "Data Source=mix-test.db";
        protected readonly MixDatabaseProvider _dbProvider = MixDatabaseProvider.SQLITE;

        public SharedDatabaseFixture()
        {
            ctor = typeof(TDbContext).GetConstructor(new Type[] { typeof(string), typeof(MixDatabaseProvider) });
            Seed();
        }

        public TDbContext CreateContext()
        {
            return (TDbContext)ctor.Invoke(new object[] { _connectionString, _dbProvider });
        }

        public void Seed()
        {
            using (var dbContext = CreateContext())
            {
                lock (_lock)
                {
                    if (!_databaseInitialized)
                    {
                        dbContext.Database.EnsureDeleted();
                        dbContext.Database.EnsureCreated();
                        dbContext.Database.Migrate();
                        SeedData(dbContext);
                        _databaseInitialized = true;
                    }
                }
            }
        }

        protected virtual void SeedData(TDbContext dbContext)
        {
        }
    }
}
