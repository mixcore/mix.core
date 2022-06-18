
using System.ComponentModel.DataAnnotations;

namespace Mix.Lib.ViewModels
{
    [GeneratePublisher]
    public class MixDomainViewModel : TenantDataViewModelBase<MixCmsContext, MixDomain, int, MixDomainViewModel>
    {
        #region Constructors

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
        [Required]
        public string Host { get; set; }
        public override void InitDefaultValues(string language = null, int? DomainId = null)
        {
            CreatedDateTime = DateTime.UtcNow;
            Status = MixContentStatus.Published;
        }
    }
}
