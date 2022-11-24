using Mix.Lib.ViewModels.ReadOnly;

namespace Mix.Lib.Base
{
    public abstract class MultilingualSEOContentViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
        : MultilingualContentViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
        where TDbContext : MixCmsContext
        where TPrimaryKey : IComparable
        where TEntity : MultilingualContentBase<TPrimaryKey>
        where TView : MultilingualSEOContentViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
    {
        #region Constructors

        protected MultilingualSEOContentViewModelBase()
        {

        }

        protected MultilingualSEOContentViewModelBase(UnitOfWorkInfo unitOfWorkInfo)
            : base(unitOfWorkInfo)
        {
        }

        protected MultilingualSEOContentViewModelBase(TEntity entity, UnitOfWorkInfo uowInfo = null)
            : base(entity, uowInfo)
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

        public override async Task ExpandView(CancellationToken cancellationToken = default)
        {
            var templateRepo = TemplateViewModel.GetRepository(UowInfo);
            if (Template == null && TemplateId.HasValue)
            {
                Template = await templateRepo.GetSingleAsync(m => m.Id == TemplateId, cancellationToken);
            }
            if (Layout == null && LayoutId.HasValue)
            {
                Layout = await templateRepo.GetSingleAsync(m => m.Id == LayoutId, cancellationToken);
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
