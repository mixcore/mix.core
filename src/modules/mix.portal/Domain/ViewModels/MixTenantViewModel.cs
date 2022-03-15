namespace Mix.Portal.Domain.ViewModels
{
    public class MixTenantViewModel
        : ViewModelBase<MixCmsContext, MixTenant, int, MixTenantViewModel>
    {
        #region Properties

        public string PrimaryDomain { get; set; }
        public string SystemName { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }

        #endregion

        #region Contructors

        public MixTenantViewModel()
        {
        }

        public MixTenantViewModel(MixTenant entity,

            UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
        {
        }

        public MixTenantViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        #endregion

        #region Overrides

        #endregion
    }
}
