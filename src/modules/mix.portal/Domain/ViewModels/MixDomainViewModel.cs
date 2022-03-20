
namespace Mix.Portal.Domain.ViewModels
{
    public class MixDomainViewModel : TenantDataViewModelBase<MixCmsContext, MixDomain, int, MixDomainViewModel>
    {
        public string Host { get; set; }
        public override void InitDefaultValues(string language = null, int? DomainId = null)
        {
            CreatedDateTime = DateTime.UtcNow;
            Status = MixContentStatus.Published;
        }
    }
}
