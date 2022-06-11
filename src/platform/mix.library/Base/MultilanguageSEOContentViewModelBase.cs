using Mix.Database.Entities.Base;
using Mix.Lib.ViewModels.ReadOnly;

namespace Mix.Lib.Base
{
    public abstract class MultilanguageSEOContentViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
        : MultilanguageContentViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
         where TDbContext : MixCmsContext
         where TPrimaryKey : IComparable
        where TEntity : MultiLanguageContentBase<TPrimaryKey>
        where TView : MultilanguageSEOContentViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
    {
        #region Contructors

        public MultilanguageSEOContentViewModelBase()
        {

        }

        protected MultilanguageSEOContentViewModelBase(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        protected MultilanguageSEOContentViewModelBase(TEntity entity, UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
        {
        }

        #endregion

        #region Properties

        public string Title { get; set; }
        public string Excerpt { get; set; }
        public string Content { get; set; }
        public int? LayoutId { get; set; }
        public int? TemplateId { get; set; }
        public TemplateViewModel Layout { get; set; }
        public TemplateViewModel Template { get; set; }
        public string Image { get; set; }
        public string Source { get; set; }
        public string SeoDescription { get; set; }
        public string SeoKeywords { get; set; }
        public string SeoName { get; set; }
        public string SeoTitle { get; set; }
        public DateTime? PublishedDateTime { get; set; }

        #region Extra

        public bool? IsClone { get; set; }

        #endregion

        #endregion

        #region Overrides
        public override Task<TEntity> ParseEntity()
        {
            return base.ParseEntity();
        }

        public override async Task ExpandView()
        {
            var templateRepo = TemplateViewModel.GetRepository(UowInfo);
            if (Template == null && TemplateId.HasValue)
            {
                Template = await templateRepo.GetSingleAsync(m => m.Id == TemplateId);
            }
            if (Layout == null && LayoutId.HasValue)
            {
                Layout = await templateRepo.GetSingleAsync(m => m.Id == LayoutId);
            }
        }

        public override void InitDefaultValues(string language = null, int? cultureId = null)
        {
            Status = MixContentStatus.Published;
            Specificulture = language ?? Specificulture;
            MixCultureId = cultureId ?? 1;
        }
        #endregion
    }
}
