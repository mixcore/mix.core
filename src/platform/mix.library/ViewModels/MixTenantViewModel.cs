namespace Mix.Lib.ViewModels
{
    public class MixTenantViewModel : ViewModelBase<MixCmsContext, MixTenant, int, MixTenantViewModel>
    {
        #region Properties

        public string SystemName { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }

        #endregion

        #region contructors

        public MixTenantViewModel()
        {
        }

        public MixTenantViewModel(MixCmsContext context) : base(context)
        {
        }

        public MixTenantViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public MixTenantViewModel(MixTenant entity, MixCacheService cacheService = null, UnitOfWorkInfo uowInfo = null) : base(entity, cacheService, uowInfo)
        {
        }

        #endregion
    }
}
