using Mix.Database.Entities.Account;

namespace Mix.Identity.ViewModels
{
    public class PermissionViewModel : ViewModelBase<MixCmsAccountContext, MixPermission, int, PermissionViewModel>
    {
        #region Contructors

        public PermissionViewModel()
        {
        }
        public PermissionViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public PermissionViewModel(MixPermission entity, UnitOfWorkInfo uowInfo) : base(entity, uowInfo)
        {
        }

        #endregion

        #region Properties

        public int TenantId { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Type { get; set; }

        #endregion
    }
}
