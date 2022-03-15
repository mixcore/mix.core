using Microsoft.EntityFrameworkCore;

namespace Mix.Lib.Base
{
    public abstract class MultilanguageContentViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
        : ViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
         where TDbContext : DbContext
         where TPrimaryKey : IComparable
        where TEntity : class, IEntity<TPrimaryKey>
        where TView : ViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
    {
        #region Contructors

        public MultilanguageContentViewModelBase()
        {

        }

        protected MultilanguageContentViewModelBase(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        protected MultilanguageContentViewModelBase(TEntity entity,
            UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
        {
        }

        #endregion

        #region Properties
        public int MixTenantId { get; set; }

        public string Specificulture { get; set; }

        public TPrimaryKey ParentId { get; set; }
        public int MixCultureId { get; set; }

        #endregion

        #region Overrides
        public override Task<TEntity> ParseEntity()
        {
            MixCultureId = MixCultureId == 0 ? 1 : MixCultureId;
            return base.ParseEntity();
        }

        public override void InitDefaultValues(string language = null, int? cultureId = null)
        {
            base.InitDefaultValues(language, cultureId);
            Specificulture ??= language;
            MixCultureId = cultureId ?? 1;
        }

        #endregion

    }
}
