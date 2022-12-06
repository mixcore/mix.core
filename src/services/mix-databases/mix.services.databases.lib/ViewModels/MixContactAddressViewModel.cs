using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;
using Mix.Services.Databases.Lib.Entities;

namespace Mix.Services.Databases.Lib.ViewModels
{
    public class MixContactAddressViewModel : ViewModelBase<MixServiceDatabaseDbContext, MixContactAddress, int, MixContactAddressViewModel>
    {
        #region Properties
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public bool IsDefault { get; set; }
        public string? Street { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public int SysUserDataId { get; set; }
        public int MixTenantId { get; set; }

        #endregion

        #region Constructors
        public MixContactAddressViewModel()
        {
        }

        public MixContactAddressViewModel(MixServiceDatabaseDbContext context) : base(context)
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
