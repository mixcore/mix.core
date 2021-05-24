using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Account;
using Mix.Heart.Infrastructure.ViewModels;
using Newtonsoft.Json;

namespace Mix.Cms.Lib.ViewModels.Account
{
    public class UserRoleViewModel
        : ViewModelBase<MixCmsAccountContext, AspNetUserRoles, UserRoleViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("roleId")]
        public string RoleId { get; set; }

        [JsonProperty("applicationUserId")]
        public string ApplicationUserId { get; set; }

        #endregion Models

        #region Views

        [JsonProperty("role")]
        public RoleViewModel UserRole { get; set; }

        #endregion Views

        #endregion Properties

        #region Contructors

        public UserRoleViewModel() : base()
        {
        }

        public UserRoleViewModel(AspNetUserRoles model, MixCmsAccountContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override void ExpandView(MixCmsAccountContext _context = null, IDbContextTransaction _transaction = null)
        {
            UserRole = RoleViewModel.Repository.GetSingleModel(r => r.Id == RoleId, _context, _transaction).Data;
        }

        #endregion Overrides
    }
}