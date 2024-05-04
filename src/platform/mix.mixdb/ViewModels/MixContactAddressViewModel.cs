using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;

namespace Mix.Mixdb.ViewModels
{
    public class MixContactAddressViewModel : ViewModelBase<MixDbDbContext, MixContactAddress, int, MixContactAddressViewModel>
    {
        #region Properties
        public bool IsDefault { get; set; }
        public string? Street { get; set; }
        public string? District { get; set; }
        public string? Ward { get; set; }
        public string? City { get; set; }
        public string? Province { get; set; }
        public int SysUserDataId { get; set; }
        public int MixTenantId { get; set; }

        #endregion

        #region Constructors
        public MixContactAddressViewModel()
        {
        }

        public MixContactAddressViewModel(MixDbDbContext context) : base(context)
        {
        }

        public MixContactAddressViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public MixContactAddressViewModel(MixContactAddress entity, UnitOfWorkInfo uowInfo) : base(entity, uowInfo)
        {
        }
        #endregion

        #region Overrides
        #endregion
    }
}
