using Mix.Heart.Entities;

namespace Mix.Lib.Base
{
    public abstract class ExtraColumnMultilanguageSEOContentViewModelBase
        <TDbContext, TEntity, TPrimaryKey, TView> 
        : HaveParentContentViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
         where TDbContext : MixCmsContext
         where TPrimaryKey : IComparable
        where TEntity : class, IEntity<TPrimaryKey>
        where TView : ViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
    {
        #region Contructors

        public ExtraColumnMultilanguageSEOContentViewModelBase()
        {

        }

        protected ExtraColumnMultilanguageSEOContentViewModelBase(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        protected ExtraColumnMultilanguageSEOContentViewModelBase(TEntity entity,
            MixCacheService cacheService = null,
            UnitOfWorkInfo uowInfo = null) : base(entity, cacheService, uowInfo)
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
