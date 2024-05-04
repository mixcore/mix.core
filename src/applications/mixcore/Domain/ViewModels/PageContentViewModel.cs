using Microsoft.EntityFrameworkCore;
using Mix.Heart.Helpers;
using Mix.RepoDb.Interfaces;
using Mix.Services.Databases.Lib.Interfaces;
using Mix.Shared.Models;

namespace Mixcore.Domain.ViewModels
{
    public sealed class PageContentViewModel
        : ExtraColumnMultilingualSEOContentViewModelBase
            <MixCmsContext, MixPageContent, int, PageContentViewModel>
    {
        #region Constructors

        public PageContentViewModel()
        {
        }

        public PageContentViewModel(MixPageContent entity,

            UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
        {
        }

        public PageContentViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }
        #endregion

        #region Properties
        public int? PageSize { get; set; }
        public MixPageType Type { get; set; }

        public string ClassName { get; set; }

        public string DetailUrl => $"/page/{Id}/{SeoName}";

        public Guid? AdditionalDataId { get; set; }

        public List<ModuleContentViewModel> Modules { get; set; }
        public PagingResponseModel<PagePostAssociationViewModel> Posts { get; set; }
        public JObject AdditionalData { get; set; }
        #endregion

        #region Overrides
        public override async Task ExpandView(CancellationToken cancellationToken = default)
        {
            await base.ExpandView(cancellationToken);
        }

        #endregion

        #region Public Method

        public async Task LoadDataAsync(IMixDbDataService mixDbDataService,
                            IMixMetadataService metadataService,
                            PagingRequestModel pagingModel, MixCacheService cacheService)
        {
            await LoadAdditionalDataAsync(mixDbDataService);
            await LoadModulesAsync(mixDbDataService, metadataService, cacheService);
            await LoadPostsAsync(pagingModel, mixDbDataService, metadataService, cacheService);
        }


        public ModuleContentViewModel GetModule(string moduleName)
        {
            return Modules?.FirstOrDefault(m => m.SystemName == moduleName);
        }
        public T Property<T>(string fieldName)
        {
            return AdditionalData != null
                ? AdditionalData.Value<T>(fieldName)
                : default;
        }

        #endregion

        #region Private Methods
        private async Task LoadAdditionalDataAsync(IMixDbDataService mixDbDataService)
        {
            if (!string.IsNullOrEmpty(MixDatabaseName))
            {
                var obj = await mixDbDataService.GetSingleByParent(MixDatabaseName, MixContentType.Page, Id, true);
                AdditionalData = obj != null ? ReflectionHelper.ParseObject(obj) : null;
            }
        }
        private async Task LoadModulesAsync(IMixDbDataService mixDbDataService, IMixMetadataService metadataService, MixCacheService cacheService)
        {
            var moduleIds = await Context.MixPageModuleAssociation
                    .AsNoTracking()
                    .Where(p => p.ParentId == Id)
                    .OrderBy(m => m.Priority)
                    .Select(m => m.ChildId)
                    .ToListAsync();
            var moduleRepo = ModuleContentViewModel.GetRepository(UowInfo, CacheService);
            Modules = await moduleRepo.GetListAsync(m => moduleIds.Contains(m.Id));
            var paging = new PagingModel();
            foreach (var item in Modules)
            {
                await item.LoadData(paging, mixDbDataService, metadataService, cacheService);
            }
        }
        private async Task LoadPostsAsync(PagingRequestModel pagingModel, IMixDbDataService mixDbDataService, IMixMetadataService metadataService, MixCacheService cacheService)
        {
            Posts = await PagePostAssociationViewModel.GetRepository(UowInfo, CacheService).GetPagingAsync(m => m.ParentId == Id, pagingModel);
            foreach (var item in Posts.Items)
            {
                item.SetUowInfo(UowInfo, CacheService);
                await item.LoadPost(mixDbDataService, metadataService, cacheService);
            }
        }


        #endregion
    }
}
