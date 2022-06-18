using Microsoft.EntityFrameworkCore;
using Mix.Database.Entities.Base;
using Mix.Lib.Entities.Base;

namespace Mix.Lib.Base
{
    public abstract class MultilingualContentViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
        : ViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
         where TDbContext : DbContext
         where TPrimaryKey : IComparable
        where TEntity : MultilingualContentBase<TPrimaryKey>
        where TView : MultilingualContentViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
    {
        #region Constructors

        public MultilingualContentViewModelBase()
        {

        }

        protected MultilingualContentViewModelBase(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        protected MultilingualContentViewModelBase(TEntity entity,
            UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
        {
        }

        #endregion

        #region Properties
        public int MixTenantId { get; set; }
        public string Icon { get; set; }
        public bool IsPublic { get; set; } = true;
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
