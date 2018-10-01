using Microsoft.EntityFrameworkCore.Storage;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using Mix.Cms.Lib.Models.Account;

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
        #endregion

        #region Views

        [JsonProperty("role")]
        public RoleViewModel Role { get; set; }

        #endregion
        #endregion

        #region Contructors

        public UserRoleViewModel() : base()
        {
        }

        public UserRoleViewModel(AspNetUserRoles model, MixCmsAccountContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
        }

        #endregion

        #region Overrides

        public override void ExpandView(MixCmsAccountContext _context = null, IDbContextTransaction _transaction = null)
        {
            Role = RoleViewModel.Repository.GetSingleModel(r => r.Id == RoleId, _context, _transaction).Data;
        }
        #endregion

        #region Expands

        #endregion

    }
}
