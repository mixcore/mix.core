
namespace Mix.Portal.Domain.ViewModels
{
    [GenerateRestApiController]
    public class MixPageContentViewModel 
        : ExtraColumnMultilanguageSEOContentViewModelBase
            <MixCmsContext, MixPageContent, int, MixPageContentViewModel>
    {
        #region Contructors

        public MixPageContentViewModel()
        {
        }

        public MixPageContentViewModel(MixPageContent entity,
            MixCacheService cacheService = null,
            UnitOfWorkInfo uowInfo = null) : base(entity, cacheService, uowInfo)
        {
        }

        public MixPageContentViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }
        #endregion

        #region Properties
        public string ClassName { get; set; }
        public int? PageSize { get; set; }
        public MixPageType Type { get; set; }
        public string DetailUrl { get; set; }

        public List<MixUrlAliasViewModel> UrlAliases { get; set; }
        #endregion

        #region Overrides
        public override async Task ExpandView(MixCacheService cacheService = null)
        {
            MixDatabaseName ??= MixDatabaseNames.PAGE_COLUMN;
            await LoadAliasAsync(cacheService);
        }

        private async Task LoadAliasAsync(MixCacheService cacheService)
        {
            var aliasRepo = MixUrlAliasViewModel.GetRepository(UowInfo);
            UrlAliases = await aliasRepo.GetListAsync(
                m => m.Type == MixUrlAliasType.Page && m.SourceContentId == Id,
                cacheService);
            DetailUrl = UrlAliases.Count > 0 ? UrlAliases[0].Alias
                : $"/page/{Id}";
        }

        public override async Task<int> CreateParentAsync()
        {
            MixPageViewModel parent = new(UowInfo)
            {
                DisplayName = Title,
                Description = Excerpt
            };
            return await parent.SaveAsync();
        }

        protected override async Task DeleteHandlerAsync()
        {
            if (Repository.GetListQuery(m => m.ParentId == ParentId).Count() == 1)
            {
                var pageRepo = MixPageViewModel.GetRepository(UowInfo);

                await Repository.DeleteAsync(Id);
                await pageRepo.DeleteAsync(ParentId);
            }
            else
            {
                await base.DeleteHandlerAsync();
            }
        }
        #endregion
    }
}
