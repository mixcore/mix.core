using Microsoft.EntityFrameworkCore;
using Mix.Lib.Services;
using System.Linq.Expressions;

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

        protected MultilingualContentViewModelBase(UnitOfWorkInfo unitOfWorkInfo)
            : base(unitOfWorkInfo)
        {
        }

        protected MultilingualContentViewModelBase(TEntity entity, UnitOfWorkInfo uowInfo = null) 
            : base(entity, uowInfo)
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

        public List<MixContributorViewModel> Contributors { get; set; }
        #endregion

        #region Overrides
        public override Task<TEntity> ParseEntity(CancellationToken cancellationToken = default)
        {
            MixCultureId = MixCultureId == 0 ? 1 : MixCultureId;
            return base.ParseEntity(cancellationToken);
        }

        public override void InitDefaultValues(string language = null, int? cultureId = null)
        {
            base.InitDefaultValues(language, cultureId);
            Specificulture ??= language;
            MixCultureId = cultureId ?? 1;
        }

        #endregion

        #region Methods

        public async Task LoadContributorsAsync(MixContentType contentType, MixIdentityService identityService)
        {
            Expression<Func<MixContributor, bool>> expression = m => m.ContentType == contentType;
            expression = expression.AndAlsoIf(Guid.TryParse(Id.ToString(), out var guidId), m => m.GuidContentId == guidId);
            expression = expression.AndAlsoIf(int.TryParse(Id.ToString(), out var integerId), m => m.IntContentId == integerId);
            Contributors = await MixContributorViewModel.GetRepository(UowInfo).GetAllAsync(expression);
            foreach (var item in Contributors)
            {
                await item.LoadUserDataAsync(identityService);
            }
        }

        #endregion

    }
}
