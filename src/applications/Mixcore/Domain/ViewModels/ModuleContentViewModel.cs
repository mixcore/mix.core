using Microsoft.EntityFrameworkCore;
using Mix.Database.Services;
using Mix.Heart.Helpers;
using Mix.RepoDb.Repositories;

namespace Mixcore.Domain.ViewModels
{
    public class ModuleContentViewModel
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
        #endregion

        #region Overrides
        public override async Task ExpandView()
        {
            await base.ExpandView();
        }

        #region Private Methods

        public async Task LoadAdditionalDataAsync(MixRepoDbRepository mixRepoDbRepository)
        {
            mixRepoDbRepository.Init(MixDatabaseName);
            var obj = await mixRepoDbRepository.GetSingleByParentAsync(MixContentType.Module, Id);
            AdditionalData = obj != null ? ReflectionHelper.ParseObject(obj) : null;
        }

        #endregion

        #endregion

        #region Public Methods

        public T Property<T>(string fieldName)
        {
            return AdditionalData != null
                ? AdditionalData.Value<T>(fieldName)
                : default;
        }

        public async Task LoadData(PagingModel pagingModel)
        {
            Data = await ModuleDataViewModel.GetRepository(UowInfo).GetPagingAsync(
                m => m.ParentId == Id,
                pagingModel);
        }
        #endregion
    }
}
