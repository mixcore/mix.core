using Microsoft.EntityFrameworkCore;
using Mix.Heart.Entities;
using Mix.Heart.Exceptions;

namespace Mix.Lib.Base
{
    public abstract class MultilanguageSEOContentViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
        : MultilanguageContentViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
         where TDbContext : MixCmsContext
         where TPrimaryKey : IComparable
        where TEntity : class, IEntity<TPrimaryKey>
        where TView : ViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
    {
        #region Contructors

        public MultilanguageSEOContentViewModelBase()
        {

        }

        protected MultilanguageSEOContentViewModelBase(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        protected MultilanguageSEOContentViewModelBase(TEntity entity,
            MixCacheService cacheService = null,
            UnitOfWorkInfo uowInfo = null) : base(entity, cacheService, uowInfo)
        {
        }

        #endregion

        #region Properties

        public string Title { get; set; }
        public string Excerpt { get; set; }
        public string Content { get; set; }
        public int? LayoutId { get; set; }
        public int? TemplateId { get; set; }
        public string Layout { get; set; }
        public string Template { get; set; }
        public string Image { get; set; }
        public string Source { get; set; }
        public string SeoDescription { get; set; }
        public string SeoKeywords { get; set; }
        public string SeoName { get; set; }
        public string SeoTitle { get; set; }
        public DateTime? PublishedDateTime { get; set; }

        #region Extra

        public bool IsClone { get; set; }

        #endregion

        #endregion

        #region Overrides
        public override Task<TEntity> ParseEntity(MixCacheService cacheService = null)
        {
            return base.ParseEntity(cacheService);
        }

        public override async Task ExpandView(MixCacheService cacheService = null, UnitOfWorkInfo uowInfo = null)
        {
            if (string.IsNullOrEmpty(Template) && TemplateId.HasValue)
            {
                var template = await Context.MixViewTemplate.FirstOrDefaultAsync(m => m.Id == TemplateId);
                Template = $"/{template.FileFolder}/{template.FileName}{template.Extension}";
            }
            if (string.IsNullOrEmpty(Layout) && LayoutId.HasValue)
            {
                var layout = await Context.MixViewTemplate.FirstOrDefaultAsync(m => m.Id == LayoutId);
                Layout = $"/{layout.FileFolder}/{layout.FileName}{layout.Extension}";
            }
        }

        public override void InitDefaultValues(string language = null, int? cultureId = null)
        {
            Status = MixContentStatus.Published;
            Specificulture = language ?? Specificulture;
            MixCultureId = cultureId ?? 1;
        }

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
