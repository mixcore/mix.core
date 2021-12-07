namespace Mix.Portal.Domain.ViewModels
{
    public class MixPageModuleViewModel
        : ViewModelBase<MixCmsContext, MixPageModuleAssociation, int, MixPageModuleViewModel>
    {
        #region Properties
        public int LeftId { get; set; }
        public int RightId { get; set; }

        #endregion

        #region Contructors

        public MixPageModuleViewModel()
        {
        }

        public MixPageModuleViewModel(MixPageModuleAssociation entity,
            MixCacheService cacheService = null,
            UnitOfWorkInfo uowInfo = null)
            : base(entity, cacheService, uowInfo)
        {
        }

        public MixPageModuleViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        #endregion

        #region Overrides

        #endregion

        #region Expands

        #endregion
    }
}
