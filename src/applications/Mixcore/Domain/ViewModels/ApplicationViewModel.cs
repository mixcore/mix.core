using Mix.Heart.Helpers;
using Mix.Lib.ViewModels.ReadOnly;
using Mix.RepoDb.Repositories;

namespace Mixcore.Domain.ViewModels
{
    [GenerateRestApiController(QueryOnly = true)]
    public sealed class ApplicationViewModel
        : TenantDataViewModelBase
            <MixCmsContext, MixApplication, int, ApplicationViewModel>
    {
        #region Constructors

        public ApplicationViewModel()
        {
        }

        public ApplicationViewModel(MixApplication entity,

            UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
        {
        }

        public ApplicationViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }
        #endregion

        #region Properties
        public string Title { get; set; }
        public string BaseHref { get; set; }
        public string BaseRoute { get; set; }
        public string Domain { get; set; }
        public string BaseApiUrl { get; set; }
        public int? TemplateId { get; set; }
        public string MixDatabaseName { get; set; }
        public Guid? MixDataContentId { get; set; }

        public JObject AdditionalData { get; set; }
        public TemplateViewModel Template { get; set; }
        #endregion

        #region Overrides
        public override async Task ExpandView(CancellationToken cancellationToken = default)
        {
            var templateRepo = TemplateViewModel.GetRepository(UowInfo);
            if (Template == null && TemplateId.HasValue)
            {
                Template = await templateRepo.GetSingleAsync(m => m.Id == TemplateId, cancellationToken);
            }
        }


        #endregion

        #region Public Method

        public T Property<T>(string fieldName)
        {
            return AdditionalData != null
                ? AdditionalData.Value<T>(fieldName)
                : default;
        }

        #endregion

        #region Private Methods
        public async Task LoadAdditionalDataAsync(MixRepoDbRepository mixRepoDbRepository)
        {
            mixRepoDbRepository.InitTableName(MixDatabaseName);
            var obj = await mixRepoDbRepository.GetSingleByParentAsync(MixContentType.Page, Id);
            AdditionalData = obj != null ? ReflectionHelper.ParseObject(obj) : null;
        }

        #endregion
    }
}
