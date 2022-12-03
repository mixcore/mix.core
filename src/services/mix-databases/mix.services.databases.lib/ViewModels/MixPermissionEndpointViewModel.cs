using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;
using Mix.Services.Databases.Lib.Entities;

namespace Mix.Services.Databases.Lib.ViewModels
{
    public class MixPermissionEndpointViewModel : ViewModelBase<MixServiceDatabaseDbContext, MixPermissionEndpoint, int, MixPermissionEndpointViewModel>
    {
        #region Properties
        public string Title { get; set; }
        public string Path { get; set; }
        public string Method { get; set; }
        public string Description { get; set; }
        public int SysPermissionId { get; set; }
        public int MixTenantId { get; set; }
        #endregion
        #region Contructors
        public MixPermissionEndpointViewModel()
        {
        }

        public MixPermissionEndpointViewModel(MixServiceDatabaseDbContext context) : base(context)
        {
        }

        public MixPermissionEndpointViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public MixPermissionEndpointViewModel(MixPermissionEndpoint entity, UnitOfWorkInfo uowInfo) : base(entity, uowInfo)
        {
        }
        #endregion
    }
}
