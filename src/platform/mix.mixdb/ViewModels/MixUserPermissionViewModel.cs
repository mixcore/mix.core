using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;

namespace Mix.Mixdb.ViewModels
{
    public class MixUserPermissionViewModel : ViewModelBase<MixDbDbContext, MixUserPermission, int, MixUserPermissionViewModel>
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

        public MixUserPermissionViewModel(MixDbDbContext context) : base(context)
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

        public override async Task ExpandView(CancellationToken cancellationToken = default)
        {
            Permission = await MixPermissionViewModel.GetRepository(UowInfo, CacheService)
                        .GetSingleAsync(m => m.Id == PermissionId && m.MixTenantId == MixTenantId);
        }

        #endregion
    }
}
