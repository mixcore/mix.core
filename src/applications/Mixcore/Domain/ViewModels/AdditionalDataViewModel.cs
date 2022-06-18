namespace Mixcore.Domain.ViewModels
{
    public class AdditionalDataViewModel
        : HaveParentContentViewModelBase<MixCmsContext, MixDataContent, Guid, AdditionalDataViewModel>
    {
        #region Constructors

        public AdditionalDataViewModel()
        {
        }

        public AdditionalDataViewModel(MixDataContent entity) : base(entity)
        {
        }

        public AdditionalDataViewModel(UnitOfWorkInfo unitOfWorkInfo = null) : base(unitOfWorkInfo)
        {
        }

        public AdditionalDataViewModel(MixDataContent entity, UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
        {
        }

        #endregion

        #region Properties

        public int MixDatabaseId { get; set; }
        public string MixDatabaseName { get; set; }
        public List<MixDatabaseColumnViewModel> Columns { get; set; }
        public List<MixDataContentValueViewModel> Values { get; set; }
        public JObject Data { get; set; }

        public List<MixDataContentViewModel> ChildData { get; set; } = new();
        public List<MixDataContentAssociationViewModel> RelatedData { get; set; } = new();

        public Guid? ContentGuidParentId { get; set; }
        public int? ContentIntParentId { get; set; }
        public MixDatabaseParentType ContentParentType { get; set; }

        #endregion

        #region Overrides

        public override async Task ExpandView()
        {
            using var colRepo = MixDatabaseColumnViewModel.GetRepository(UowInfo);
            using var valRepo = MixDataContentValueViewModel.GetRepository(UowInfo);

            Values = await valRepo.GetListAsync(m => m.ParentId == Id);

            if (Data == null)
            {
                Data = MixDataHelper.ParseData(Id, UowInfo);
            }

            await Data.LoadAllReferenceDataAsync(Id, MixDatabaseName, UowInfo);
        }

        #endregion

        #region Public Methods

        public bool HasValue(string fieldName)
        {
            return Data?.Value<string>(fieldName) != null;
        }

        public T Property<T>(string fieldName)
        {
            if (Data != null)
            {
                return Data.Value<T>(fieldName);
            }
            else
            {
                return default;
            }
        }

        #endregion
    }
}
