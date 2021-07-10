using Mix.Database.Entities.Account;
using Mix.Heart.Enums;
using Mix.Heart.ViewModel;
using System;
using System.Threading.Tasks;

namespace Mix.Identity.ViewModels
{
    public class RoleViewModel : ViewModelBase<MixCmsAccountContext, AspNetRoles, Guid>
    {
        public RoleViewModel(AspNetRoles entity) : base(entity)
        {
        }

        #region Properties

        public string ConcurrencyStamp { get; set; }

        public string Name { get; set; }

        public string NormalizedName { get; set; }

        #region Views

        //public List<MixDatabaseDatas.ReadMvcViewModel> MixPermissions { get; set; }
        #endregion Views

        #endregion Properties

        #region Overrides
        public override Task<AspNetRoles> ParseEntity<T>(T view)
        {
            if (view.Id == Guid.Empty)
            {
                view.Id = Guid.NewGuid();
            }
            return base.ParseEntity(view);
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
