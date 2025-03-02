using Mix.Database.Entities.Account;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mix.Identity.ViewModels
{
    public class RoleViewModel : ViewModelBase<MixCmsAccountContext, MixRole, Guid, RoleViewModel>
    {
        #region Constructors
        public RoleViewModel()
        {
        }

        public RoleViewModel(MixRole entity, UnitOfWorkInfo uowInfo)
            : base(entity, uowInfo)
        {
        }

        public RoleViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        #endregion

        #region Properties

        public string ConcurrencyStamp { get; set; }

        public string Name { get; set; }

        public string NormalizedName { get; set; }

        #region Views

        //public List<MixDatabaseDatas.ReadMvcViewModel> MixPermissions { get; set; }
        #endregion Views

        #endregion Properties

        #region Overrides
        public override Task<MixRole> ParseEntity(CancellationToken cancellationToken = default)
        {
            if (Id == Guid.Empty)
            {
                Id = Guid.NewGuid();
            }
            return base.ParseEntity(cancellationToken);
        }

        public override void InitDefaultValues(string language = null, int? cultureId = null)
        {
            Id = Guid.NewGuid();
            CreatedDateTime = DateTime.UtcNow;
            Status = MixContentStatus.Published;
        }

        #endregion Overrides

        #region Expands

        #endregion Expands
    }
}
