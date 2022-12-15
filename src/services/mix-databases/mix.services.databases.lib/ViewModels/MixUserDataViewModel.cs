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
        public string? PhoneNumber { get; set; }
        public string? Fullname { get; set; }
        public string? Avatar { get; set; }
        public string? Gender { get; set; }
        public int MixTenantId { get; set; }

        public List<MixContactAddressViewModel>? Addresses { get; set; }
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

        protected override async Task SaveEntityRelationshipAsync(MixUserData parentEntity, CancellationToken cancellationToken = default)
        {
            await SaveAddresses(parentEntity, cancellationToken);
        }


        #endregion

        #region Methods

        private async Task SaveAddresses(MixUserData parentEntity, CancellationToken cancellationToken)
        {
            if (Addresses!= null)
            {
                foreach (var item in Addresses)
                {
                    item.SetUowInfo(UowInfo);
                    item.SysUserDataId = parentEntity.Id;
                    await item.SaveAsync(cancellationToken);
                }
            }
        }


        #endregion
    }
}
