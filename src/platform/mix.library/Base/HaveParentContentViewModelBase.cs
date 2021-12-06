using Mix.Heart.Entities;
using Mix.Heart.Exceptions;
using Mix.Lib.ViewModels.ReadOnly;

namespace Mix.Lib.Base
{
    public abstract class HaveParentContentViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
        : MultilanguageSEOContentViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
         where TDbContext : MixCmsContext
         where TPrimaryKey : IComparable
        where TEntity : class, IEntity<TPrimaryKey>
        where TView : ViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
    {
        #region Contructors

        public HaveParentContentViewModelBase()
        {

        }

        protected HaveParentContentViewModelBase(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        protected HaveParentContentViewModelBase(TEntity entity,
            MixCacheService cacheService = null,
            UnitOfWorkInfo uowInfo = null) : base(entity, cacheService, uowInfo)
        {
        }

        #endregion

        #region Overrides

        protected override async Task<TEntity> SaveHandlerAsync()
        {
            if (IsDefaultId(ParentId))
            {
                ParentId = await CreateParentAsync();
            }
            return await base.SaveHandlerAsync();
        }

        #endregion

        public virtual Task<TPrimaryKey> CreateParentAsync()
        {
            throw new MixException($"Not implemented CreateParentAsync: {typeof(TView).FullName}");
        }
    }
}
