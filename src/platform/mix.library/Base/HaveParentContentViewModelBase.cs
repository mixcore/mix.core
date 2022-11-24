namespace Mix.Lib.Base
{
    public abstract class HaveParentContentViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
        : MultilingualContentViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
        where TDbContext : MixCmsContext
        where TPrimaryKey : IComparable
        where TEntity : MultilingualContentBase<TPrimaryKey>
        where TView : HaveParentContentViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
    {
        #region Constructors

        protected HaveParentContentViewModelBase()
        {
        }

        protected HaveParentContentViewModelBase(UnitOfWorkInfo unitOfWorkInfo)
            : base(unitOfWorkInfo)
        {
        }

        protected HaveParentContentViewModelBase(TEntity entity, UnitOfWorkInfo uowInfo = null)
            : base(entity, uowInfo)
        {
        }

        #endregion

        #region Overrides

        protected override async Task<TEntity> SaveHandlerAsync(CancellationToken cancellationToken = default)
        {
            if (IsDefaultId(ParentId))
            {
                ParentId = await CreateParentAsync(cancellationToken);
            }

            return await base.SaveHandlerAsync(cancellationToken);
        }

        #endregion

        public virtual Task<TPrimaryKey> CreateParentAsync(CancellationToken cancellationToken = default)
        {
            throw new MixException($"Not implemented CreateParentAsync: {typeof(TView).FullName}");
        }
    }
}
