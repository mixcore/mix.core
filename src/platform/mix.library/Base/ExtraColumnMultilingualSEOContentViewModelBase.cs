namespace Mix.Lib.Base
{
    public abstract class ExtraColumnMultilingualSEOContentViewModelBase
        <TDbContext, TEntity, TPrimaryKey, TView>
        : HaveParentContentViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
         where TDbContext : MixCmsContext
         where TPrimaryKey : IComparable
        where TEntity : MultilingualContentBase<TPrimaryKey>
        where TView : ExtraColumnMultilingualSEOContentViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
    {
        #region Constructors

        public ExtraColumnMultilingualSEOContentViewModelBase()
        {

        }

        protected ExtraColumnMultilingualSEOContentViewModelBase(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        protected ExtraColumnMultilingualSEOContentViewModelBase(TEntity entity,
            UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
        {
        }

        #endregion

        #region Properties

        public string MixDatabaseName { get; set; }
        public Guid? MixDataContentId { get; set; }

        #endregion

        #region Overrides

        public override void InitDefaultValues(string language = null, int? cultureId = null)
        {
            base.InitDefaultValues(language, cultureId);
        }

        #endregion

    }
}
