using Mix.Constant.Enums;
using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;
using Mix.Services.Databases.Lib.Entities;

namespace Mix.Services.Databases.Lib.ViewModels
{
    public class MixUserDataViewModel : ViewModelBase<MixServiceDatabaseDbContext, MixUserData, int, MixUserDataViewModel>
    {
        #region Properties

        public Guid ParentId { get; set; }
        public MixDatabaseParentType ParentType { get; set; }
        public string Avatar { get; set; }
        public int MixTenantId { get; set; }

        public List<MixContactAddressViewModel> Addresses { get; set; }
        #endregion
        #region Contructors
        public MixUserDataViewModel()
        {
        }

        public MixUserDataViewModel(MixServiceDatabaseDbContext context) : base(context)
        {
        }

        public MixUserDataViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public MixUserDataViewModel(MixUserData entity, UnitOfWorkInfo uowInfo) : base(entity, uowInfo)
        {
        }
        #endregion
        #region Overrides

        public override async Task ExpandView(CancellationToken cancellationToken = default)
        {
            Addresses = await MixContactAddressViewModel.GetRepository(UowInfo)
                        .GetListAsync(m => m.SysUserDataId == Id && m.MixTenantId == MixTenantId);
        }

        #endregion
    }
}
