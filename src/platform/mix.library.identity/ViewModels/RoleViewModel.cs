using Mix.Database.Entities.Account;
using Mix.Heart.Enums;
using Mix.Heart.Repository;
using Mix.Heart.UnitOfWork;
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

        public RoleViewModel(MixCmsAccountContext context) : base(context)
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

        protected override void InitEntityValues()
        {
            if (Id == default)
            {
                Id = Guid.NewGuid();
                CreatedDateTime = DateTime.UtcNow;
                Status = MixContentStatus.Published;
            }
        }

        #endregion Overrides

        #region Expands

        #endregion Expands
    }
}
