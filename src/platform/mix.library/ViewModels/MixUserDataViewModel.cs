namespace Mix.Lib.ViewModels
{
    public sealed class MixUserDataViewModel
        : HaveParentContentViewModelBase<MixCmsContext, MixDataContent, Guid, MixUserDataViewModel>
    {
        #region Constructors

        public MixUserDataViewModel()
        {
        }

        public MixUserDataViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public MixUserDataViewModel(MixDataContent entity,
            UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
        {
        }


        #endregion

        #region Properties
        public int MixDatabaseId { get; set; }
        public string MixDatabaseName { get; set; }
        public JObject Data { get; set; }

        public Guid? GuidParentId { get; set; }
        public int? IntParentId { get; set; }
        public MixDatabaseParentType ParentType { get; set; }
        #endregion

        #region Overrides

        public override async Task ExpandView()
        {
            if (Data == null)
            {

                using var colRepo = MixDatabaseColumnViewModel.GetRepository(UowInfo);
                using var valRepo = MixDataContentValueViewModel.GetRepository(UowInfo);

                var Columns = await colRepo.GetListAsync(m => m.MixDatabaseName == MixDatabaseName);
                var Values = await valRepo.GetListAsync(m => m.ParentId == Id);

                Data = MixDataHelper.ParseData(Id, UowInfo);

                await Data.LoadAllReferenceDataAsync(Id, MixDatabaseName, UowInfo);
            }
        }

        public override async Task<Guid> CreateParentAsync()
        {
            MixDataViewModel parent = new(UowInfo)
            {
                Id = Guid.NewGuid(),
                CreatedDateTime = DateTime.UtcNow,
                MixTenantId = MixTenantId,
                MixDatabaseId = MixDatabaseId,
                MixDatabaseName = MixDatabaseName,
                CreatedBy = CreatedBy,
                DisplayName = Title,
                Description = Excerpt
            };
            return await parent.SaveAsync();
        }
        #endregion
    }
}
