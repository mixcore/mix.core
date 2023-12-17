using Mix.Heart.Helpers;
using Mix.Lib.ViewModels.ReadOnly;
using Mix.RepoDb.Repositories;

namespace Mixcore.Domain.ViewModels
{
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
        public string BaseHref { get; set; }
        public string DeployUrl { get; set; }
        public JObject AppSettings { get; set; }
        public string Domain { get; set; }
        public string BaseApiUrl { get; set; }
        public int? TemplateId { get; set; }
        public string MixDatabaseName { get; set; }
        public int? MixDbId { get; set; }

        public JObject ExtraData { get; set; }
        public TemplateViewModel Template { get; set; }
        #endregion

        #region Overrides
        public override async Task ExpandView(CancellationToken cancellationToken = default)
        {
            var templateRepo = TemplateViewModel.GetRepository(UowInfo, CacheService);
            if (Template == null && TemplateId.HasValue)
            {
                Template = await templateRepo.GetSingleAsync(m => m.Id == TemplateId, cancellationToken);
            }
        }


        #endregion

        #region Public Method

        public T Property<T>(string fieldName)
        {
            return ExtraData != null
                ? ExtraData.Value<T>(fieldName)
                : default;
        }

        #endregion

        #region Private Methods
        public async Task LoadAdditionalDataAsync(MixRepoDbRepository mixRepoDbRepository)
        {
            mixRepoDbRepository.InitTableName(MixDatabaseName);
            var obj = await mixRepoDbRepository.GetSingleByParentAsync(MixContentType.Page, Id);
            ExtraData = obj != null ? ReflectionHelper.ParseObject(obj) : null;
        }

        #endregion
    }
}
