using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;
using Mix.Lib.Attributes;
using Mix.Services.Permission.Domain.Entities;

namespace Mix.Services.Permission.Domain.ViewModels
{
    public class MixUserPermissionViewModel : ViewModelBase<PermissionDbContext, MixUserPermission, int, MixUserPermissionViewModel>
    {
        #region Properties

        public Guid UserId { get; set; }
        public int PermissionId { get; set; }
        public string Description { get; set; }
        public int MixTenantId { get; set; }
        public MixPermissionViewModel Permission { get; set; }
        #endregion
        #region Contructors
        public MixUserPermissionViewModel()
        {
        }

        public MixUserPermissionViewModel(PermissionDbContext context) : base(context)
        {
        }

        public MixUserPermissionViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public MixUserPermissionViewModel(MixUserPermission entity, UnitOfWorkInfo uowInfo) : base(entity, uowInfo)
        {
        }
        #endregion
        #region Overrides

        public override async Task ExpandView()
        {
            Permission = await MixPermissionViewModel.GetRepository(UowInfo)
                        .GetSingleAsync(m => m.Id == PermissionId && m.MixTenantId == MixTenantId);
        }

        #endregion
    }
}
