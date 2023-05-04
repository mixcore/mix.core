using Mix.Heart.Helpers;
using Mix.RepoDb.Repositories;
using Mix.Services.Databases.Lib.Interfaces;

namespace Mixcore.Domain.ViewModels
{
    public sealed class ModuleContentViewModel
        : ExtraColumnMultilingualSEOContentViewModelBase
            <MixCmsContext, MixModuleContent, int, ModuleContentViewModel>
    {
        #region Constructors

        public ModuleContentViewModel()
        {
        }

        public ModuleContentViewModel(MixModuleContent entity,
            UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
        {
        }

        public ModuleContentViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }
        #endregion

        #region Properties

        public string SystemName { get; set; }
        public string ClassName { get; set; }
        public int? PageSize { get; set; }
        public MixModuleType Type { get; set; }

        public string DetailUrl => $"/Module/{Id}/{SeoName}";

        public JObject AdditionalData { get; set; }
        public PagingResponseModel<ModuleDataViewModel> Data { get; set; }
        public PagingResponseModel<ModulePostAssociationViewModel> Posts { get; set; }
        #endregion

        #region Overrides
        public override async Task ExpandView(CancellationToken cancellationToken = default)
        {
            await base.ExpandView(cancellationToken);
        }

        #region Private Methods

        #endregion

        #endregion

        #region Public Methods

        public T Property<T>(string fieldName)
        {
            return AdditionalData != null
                ? AdditionalData.Value<T>(fieldName)
                : default;
        }

        private async Task LoadAdditionalDataAsync(MixRepoDbRepository mixRepoDbRepository)
        {
            if (!string.IsNullOrEmpty(MixDatabaseName))
            {
                mixRepoDbRepository.InitTableName(MixDatabaseName);
                var obj = await mixRepoDbRepository.GetSingleByParentAsync(MixContentType.Page, Id);
                AdditionalData = obj != null ? ReflectionHelper.ParseObject(obj) : null;
            }
        }

        public async Task LoadData(PagingModel pagingModel, MixRepoDbRepository mixRepoDbRepository, IMixMetadataService metadataService,
            MixCacheService cacheService)
        {
            await LoadAdditionalDataAsync(mixRepoDbRepository);
            Data = await ModuleDataViewModel.GetRepository(UowInfo, CacheService).GetPagingAsync(
                m => m.ParentId == Id,
                pagingModel);
            Posts = await ModulePostAssociationViewModel.GetRepository(UowInfo, CacheService).GetPagingAsync(
                m => m.ParentId == Id,
                pagingModel);
            foreach (var item in Posts.Items)
            {
                await item.Post.LoadAdditionalDataAsync(mixRepoDbRepository, metadataService, cacheService);
            }
        }
        #endregion
    }
}
