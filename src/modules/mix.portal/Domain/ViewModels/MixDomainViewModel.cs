namespace Mix.Portal.Domain.ViewModels
{
    [GenerateRestApiController]
    public class MixDomainViewModel
        : TenantDataViewModelBase<MixCmsContext, MixDomain, int, MixDomainViewModel>
    {
        #region Contructors

        public MixDomainViewModel()
        {
        }

        public MixDomainViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public MixDomainViewModel(MixDomain entity, UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
        {
        }

        #endregion

        #region Properties

        public string Host { get; set; }

        #endregion

        #region Overrides

        #endregion
    }
}
