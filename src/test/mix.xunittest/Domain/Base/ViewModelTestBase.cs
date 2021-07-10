using Microsoft.EntityFrameworkCore;
using Mix.Heart.Entities;
using Mix.Heart.Enums;
using Mix.Heart.Exceptions;
using Mix.Heart.Helpers;
using Mix.Heart.Repository;
using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;
using System;
using System.Threading.Tasks;
using Xunit;
using XUnit.Project.Attributes;

// Ref: https://docs.microsoft.com/en-us/dotnet/core/testing/order-unit-tests
// Need to turn off test parallelization so we can validate the run order

[assembly: CollectionBehavior(DisableTestParallelization = true)]
[assembly: TestCollectionOrderer("Mix.XUnittest.Domain.Orderers.DisplayNameOrderer", "mix.xunittest")]

namespace Mix.Xunittest.Domain.Base
{
    [TestCaseOrderer("Mix.XUnit.Domain.Orderers.PriorityOrderer", "mix.xunittest")]
    public abstract class ViewModelTestBase<TFixture, TView, TDbContext, TEntity, TPrimaryKey>
         : IClassFixture<TFixture>
        where TFixture : SharedDatabaseFixture<TDbContext>
        where TView : ViewModelBase<TDbContext, TEntity, TPrimaryKey>
        where TDbContext : DbContext
        where TPrimaryKey : IComparable
        where TEntity : class, IEntity<TPrimaryKey>
    {
        public TFixture Fixture { get; set; }

        protected Repository<TDbContext, TEntity, TPrimaryKey> _repository { get; set; }
        protected UnitOfWorkInfo _uowInfo { get; set; }

        public ViewModelTestBase(TFixture fixture)
        {
            Fixture = fixture;
        }

        #region Abstracts

        protected abstract TView CreateSampleValue();

        #endregion

        [Fact, TestPriority(1)]
        public async Task Step_1_Save()
        {
            TView valueToAdd = CreateSampleValue();
            var key = await valueToAdd.SaveAsync();
            Assert.True(key != null, key.ToString());
        }

        [Fact, TestPriority(2)]
        public async Task Step_2_GetList()
        {
            using (var dbContext = Fixture.CreateContext())
            {
                _repository = new(dbContext);
                var data = await _repository.GetListViewAsync<TView>(m => true);
                Assert.True(data.Count > 0);
            }
        }

        [Fact, TestPriority(3)]
        public async Task Step_3_Delete()
        {
            using (var dbContext = Fixture.CreateContext())
            {
                try
                {
                    _repository = new(dbContext);
                    var predicate = ReflectionHelper.GetExpression<TEntity>("Id", 1, ExpressionMethod.Eq);
                    await _repository.DeleteAsync(predicate);
                    Assert.True(true);
                }
                catch (MixException mex)
                {
                    Assert.True(false, mex.Message);
                }
                catch (Exception ex)
                {
                    Assert.True(false, ex.Message);
                }
            }
        }
    }
}
