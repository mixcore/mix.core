namespace Mix.Lib.ViewModels
{
    public class MixDataContentAssociationViewModel
        : MultilingualContentViewModelBase<MixCmsContext, MixDataContentAssociation, Guid, MixDataContentAssociationViewModel>
    {
        #region Contructors

        public MixDataContentAssociationViewModel()
        {
        }

        public MixDataContentAssociationViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public MixDataContentAssociationViewModel(MixDataContentAssociation entity,

            UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
        {
        }
        #endregion

        #region Properties

        public int MixDatabaseId { get; set; }
        public string MixDatabaseName { get; set; }
        public MixDatabaseParentType ParentType { get; set; }
        public Guid DataContentId { get; set; }
        public Guid? GuidParentId { get; set; }
        public int? IntParentId { get; set; }

        public MixDataContentViewModel ChildDataContent { get; set; }

        #endregion

        #region Overrides

        public override Task<MixDataContentAssociation> ParseEntity()
        {
            if (IsDefaultId(Id))
            {
                Id = Guid.NewGuid();
                CreatedDateTime = DateTime.UtcNow;
            }
            return base.ParseEntity();
        }

        public override async Task ExpandView()
        {
            var contentRepo = MixDataContentViewModel.GetRepository(UowInfo);
            ChildDataContent = await contentRepo.GetSingleAsync(DataContentId);
        }
        #endregion

    }
}
