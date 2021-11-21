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
        private static readonly object _lock = new();
        private static bool _databaseInitialized;
        protected static ConstructorInfo ctor;
        public TDbContext Context { get; set; }
        public string ConnectionString = "Data Source=MixContent\\mix-test.db";
        public MixDatabaseProvider DbProvider = MixDatabaseProvider.SQLITE;

        public SharedDatabaseFixture()
        {
            ctor = typeof(TDbContext).GetConstructor(new Type[] { typeof(string), typeof(MixDatabaseProvider) });
            Context = CreateContext();
            //Seed();
        }

        public TDbContext CreateContext()
        {
            return (TDbContext)ctor.Invoke(new object[] { ConnectionString, DbProvider });
        }

        public void Seed()
        {
            
            lock (_lock)
            {
                if (!_databaseInitialized)
                {
                    Context.Database.EnsureDeleted();
                    Context.Database.EnsureCreated();
                    Context.Database.Migrate();
                    SeedData(Context);
                    _databaseInitialized = true;
                }
            }
        }

        protected virtual void SeedData(TDbContext dbContext)
        {
        }
    }
}
