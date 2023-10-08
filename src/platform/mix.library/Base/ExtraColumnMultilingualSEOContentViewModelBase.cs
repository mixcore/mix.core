namespace Mix.Lib.Base
{
    public abstract class ExtraColumnMultilingualSEOContentViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
        : HaveParentSEOContentViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
        where TDbContext : MixCmsContext
        where TPrimaryKey : IComparable
        where TEntity : MultilingualContentBase<TPrimaryKey>
        where TView : ExtraColumnMultilingualSEOContentViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
    {
        #region Constructors

        protected ExtraColumnMultilingualSEOContentViewModelBase()
        {

        }

        protected ExtraColumnMultilingualSEOContentViewModelBase(UnitOfWorkInfo unitOfWorkInfo)
            : base(unitOfWorkInfo)
        {
        }

        protected ExtraColumnMultilingualSEOContentViewModelBase(TEntity entity, UnitOfWorkInfo uowInfo)
            : base(entity, uowInfo)
        {
        }

        #endregion

        #region Properties

        public string MixDatabaseName { get; set; }
        public int? MixDbId { get; set; }

        #endregion
    }
}
