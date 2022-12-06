namespace Mix.Lib.ViewModels
{
    public sealed class MixDatabaseAssociationViewModel
        : ViewModelBase<MixCmsContext, MixDatabaseAssociation, Guid, MixDatabaseAssociationViewModel>
    {
        #region Properties
        public int MixTenantId { get; set; }
        public string ParentDatabaseName { get; set; }
        public string ChildDatabaseName { get; set; }
        public int ParentId { get; set; }
        public int ChildId { get; set; }
        #endregion

        #region Constructors

        public MixDatabaseAssociationViewModel()
        {
        }

        public MixDatabaseAssociationViewModel(MixDatabaseAssociation entity, UnitOfWorkInfo uowInfo = null) 
            : base(entity, uowInfo)
        {
        }

        public MixDatabaseAssociationViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        #endregion

        #region Overrides
        public override void InitDefaultValues(string language = null, int? cultureId = null)
        {
            base.InitDefaultValues(language, cultureId);
            if (Id == default)
            {
                Id = Guid.NewGuid();
            }
        }
        #endregion

        #region Expands

        #endregion
    }
}
