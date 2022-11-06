using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;
using Mix.Services.Permission.Domain.Entities;

namespace Mix.Services.Permission.Domain.ViewModels
{
    public class MixPermissionViewModel : ViewModelBase<PermissionDbContext, MixPermission, int, MixPermissionViewModel>
    {
        #region Properties

        public string Title { get; set; }
        public string Type { get; set; }
        public string Icon { get; set; }
        public int MixTenantId { get; set; }
        public List<MixPermissionEndpointViewModel> Endpoints { get; set; }
        #endregion
        #region Contructors
        public MixPermissionViewModel()
        {
        }

        public MixPermissionViewModel(PermissionDbContext context) : base(context)
        {
        }

        public MixPermissionViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public MixPermissionViewModel(MixPermission entity, UnitOfWorkInfo uowInfo) : base(entity, uowInfo)
        {
        }
        #endregion
        #region Overrides

        public override async Task ExpandView()
        {
            Endpoints = await MixPermissionEndpointViewModel.GetRepository(UowInfo)
                        .GetAllAsync(m => m.SysPermissionId == Id && m.MixTenantId == MixTenantId);
        }

        protected override async Task SaveEntityRelationshipAsync(MixPermission parentEntity)
        {
            if (Endpoints != null && Endpoints.Count() > 0)
            {
                foreach (var ep in Endpoints)
                {
                    ep.SetUowInfo(UowInfo);
                    ep.SysPermissionId = parentEntity.Id;
                    ep.CreatedBy = ModifiedBy;
                    ep.ModifiedBy = ModifiedBy;
                    ep.MixTenantId = parentEntity.MixTenantId;
                    await ep.SaveAsync();
                }
            }
        }

        #endregion
    }
}
