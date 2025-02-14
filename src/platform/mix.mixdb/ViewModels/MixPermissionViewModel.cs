using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;

namespace Mix.Mixdb.ViewModels
{
    public class MixPermissionViewModel : ViewModelBase<MixDbDbContext, MixPermission, int, MixPermissionViewModel>
    {
        #region Properties
        public int TenantId { get; set; }
        public string DisplayName { get; set; }
        public string Group { get; set; }
        public string Key { get; set; }
        public Metadata? Metadata { get; set; }
        public List<MixPermissionEndpointViewModel> Endpoints { get; set; }
        #endregion

        #region Constructors
        public MixPermissionViewModel()
        {
        }

        public MixPermissionViewModel(MixDbDbContext context) : base(context)
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

        public override async Task ExpandView(CancellationToken cancellationToken = default)
        {
            Endpoints = await MixPermissionEndpointViewModel.GetRepository(UowInfo, CacheService).GetAllAsync(m => m.SysPermissionId == Id && m.TenantId == TenantId, cancellationToken);
        }

        protected override async Task SaveEntityRelationshipAsync(MixPermission parentEntity, CancellationToken cancellationToken = default)
        {
            if (Endpoints != null && Endpoints.Count() > 0)
            {
                foreach (var endpoint in Endpoints)
                {
                    endpoint.SetUowInfo(UowInfo, CacheService);
                    endpoint.SysPermissionId = parentEntity.Id;
                    endpoint.CreatedBy = ModifiedBy;
                    endpoint.ModifiedBy = ModifiedBy;
                    endpoint.TenantId = parentEntity.TenantId;
                    await endpoint.SaveAsync(cancellationToken);
                    ModifiedEntities.AddRange(endpoint.ModifiedEntities);
                }
            }
        }

        #endregion
    }
}
