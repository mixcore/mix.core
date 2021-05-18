using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Account;
using Mix.Heart.Infrastructure.ViewModels;
using Newtonsoft.Json;
using System;

namespace Mix.Cms.Lib.ViewModels.Account
{
    public class NavUserRoleViewModel
          : ViewModelBase<MixCmsAccountContext, AspNetUserRoles, NavUserRoleViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("specificulture")]
        public string Specificulture { get; set; }

        [JsonProperty("priority")]
        public int Priority { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("roleId")]
        public string RoleId { get; set; }

        [JsonProperty("applicationUserId")]
        public string ApplicationUserId { get; set; }

        #endregion Models

        #region Views

        [JsonProperty("isActived")]
        public bool IsActived { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("role")]
        public RoleViewModel Role { get; set; }

        #endregion Views

        #endregion Properties

        #region Contructors

        public NavUserRoleViewModel() : base()
        {
            IsCache = false;
        }

        public NavUserRoleViewModel(AspNetUserRoles model, MixCmsAccountContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
            IsCache = false;
        }

        #endregion Contructors

        #region Overrides

        public override void ExpandView(MixCmsAccountContext _context = null, IDbContextTransaction _transaction = null)
        {
            try
            {
                Role = RoleViewModel.Repository.GetSingleModel(r => r.Id == RoleId, _context, _transaction).Data;
                Description = Role?.Name;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        #endregion Overrides
    }
}