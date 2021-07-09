using Microsoft.EntityFrameworkCore;
using Mix.Database.Services;
using Mix.Heart.Entities;
using Mix.Heart.Enums;
using Mix.Heart.Exceptions;
using Mix.Heart.Helpers;
using Mix.Heart.Repository;
using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;
using Mix.Shared.Services;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace Mix.XUnittest.Domain.Base
{
    public abstract class ViewModelTestBase<TView, TDbContext, TEntity, TPrimaryKey>
        where TView : ViewModelBase<TDbContext, TEntity, TPrimaryKey>
        where TDbContext : DbContext
        where TPrimaryKey : IComparable
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected Repository<TDbContext, TEntity, TPrimaryKey> _repository { get; set; }
        protected UnitOfWorkInfo _uowInfo { get; set; }

        protected const string _connectionString = "Data Source=mix-test.db";
        protected const MixDatabaseProvider _dbProvider = MixDatabaseProvider.SQLITE;
        protected static TDbContext _dbContext;
        protected static ConstructorInfo ctor;
        protected static bool _databaseInitialized;
        public ViewModelTestBase()
        {
            ctor = typeof(TDbContext).GetConstructor(new Type[] { typeof(string), typeof(MixDatabaseProvider) });
            if (!_databaseInitialized)
            {
                Seed();
            }
        }

        #region Abstracts

        protected abstract TView CreateSampleValue();

        #endregion

        #region Methods

        protected virtual void Seed()
        {
            using (_dbContext = (TDbContext)ctor.Invoke(new object[] { _connectionString, _dbProvider }))
            {
                _dbContext.Database.EnsureDeleted();
                _dbContext.Database.EnsureCreated();
                _dbContext.Database.Migrate();
                _databaseInitialized = true;
            }
            
        }

        #endregion

        [Fact]
        public async Task Step_1_Save()
        {
            TView valueToAdd = CreateSampleValue();
            var key = await valueToAdd.SaveAsync();
            Assert.True(key != null);
        }

        [Fact]
        public async Task Step_2_GetList()
        {
            using (_dbContext = (TDbContext)ctor.Invoke(new object[] { _connectionString, _dbProvider }))
            {
                _repository = new(_dbContext);
                var data = await _repository.GetListViewAsync<TView>(m => true);
                Assert.True(data.Count > 0);
            }
        }

        [Fact]
        public async Task Step_3_Delete()
        {
            using (_dbContext = (TDbContext)ctor.Invoke(new object[] { _connectionString, _dbProvider }))
            {
                try
                {
                    _repository = new(_dbContext);
                    var predicate = ReflectionHelper.GetExpression<TEntity>("Id", 1, ExpressionMethod.Eq);
                    await _repository.DeleteAsync(predicate);
                    Assert.True(true);
                }
                catch (MixException mex)
                {
                    Assert.True(false);
                }
                catch
                {
                    Assert.True(false);
                }
            }
        }
    }
}
