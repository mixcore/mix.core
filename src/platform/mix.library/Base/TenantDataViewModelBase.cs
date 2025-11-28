using Microsoft.EntityFrameworkCore;

namespace Mix.Lib.Base
{
    public abstract class TenantDataViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
        : ViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
        where TDbContext : DbContext
        where TPrimaryKey : IComparable
        where TEntity : class, IEntity<TPrimaryKey>
        where TView : ViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
    {
        #region Constructors
        protected TenantDataViewModelBase()
        {
        }

        protected TenantDataViewModelBase(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        protected TenantDataViewModelBase(TEntity entity, UnitOfWorkInfo uowInfo)
            : base(entity, uowInfo)
        {
        }

        #endregion

        #region Properties

        public string Description { get; set; }

        public virtual string Image { get; set; }
        public virtual string DisplayName { get; set; }
        public int TenantId { get; set; }

        #endregion

        #region Overrides
        #endregion
    }
}
