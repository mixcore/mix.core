using Microsoft.EntityFrameworkCore;

namespace Mix.Lib.Base
{
    public abstract class TenantDataUniqueNameViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
        : TenantDataViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
        where TDbContext : DbContext
        where TPrimaryKey : IComparable
        where TEntity : class, IEntity<TPrimaryKey>
        where TView : ViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
    {
        #region Constructors
        protected TenantDataUniqueNameViewModelBase()
        {
        }

        protected TenantDataUniqueNameViewModelBase(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        protected TenantDataUniqueNameViewModelBase(TEntity entity, UnitOfWorkInfo uowInfo = null)
            : base(entity, uowInfo)
        {
        }

        #endregion

        #region Properties

        public string SystemName { get; set; }

        #endregion

        #region Overrides

        #endregion
    }
}
