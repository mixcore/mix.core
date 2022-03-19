
using Mix.Database.Entities.Account;

namespace Mix.Portal.Domain.ViewModels
{
    public class MixRoleViewModel
        : ViewModelBase<MixCmsAccountContext, MixRole, Guid, MixRoleViewModel>
    {
        #region Contructors

        public MixRoleViewModel()
        {
        }

        public MixRoleViewModel(MixRole entity,

            UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
        {
        }

        public MixRoleViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }
        #endregion

        #region Properties

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public string ConcurrencyStamp { get; set; }

        public int MixTenantId { get; set; }

        #endregion

        #region Overrides

        #endregion
    }
}
