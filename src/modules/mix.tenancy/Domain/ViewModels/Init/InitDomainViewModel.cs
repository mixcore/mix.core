
namespace Mix.Tenancy.Domain.ViewModels.Init
{
    public class InitDomainViewModel : TenantDataViewModelBase<MixCmsContext, MixDomain, int, InitDomainViewModel>
    {
        public InitDomainViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public string Host { get; set; }
        public override void InitDefaultValues(string language = null, int? domainId = null)
        {
            CreatedDateTime = DateTime.UtcNow;
            Status = MixContentStatus.Published;
        }
    }
}
