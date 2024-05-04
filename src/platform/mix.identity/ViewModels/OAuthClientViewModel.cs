using Mix.Database.Entities.Account;
using Mix.Shared.Enums;
using System;
using System.Collections.Generic;

namespace Mix.Identity.ViewModels
{
    public sealed class OAuthClientViewModel : ViewModelBase<MixCmsAccountContext, OAuthClient, Guid, OAuthClientViewModel>
    {
        #region Properties
        public string Name { get; set; }
        public ApplicationType ApplicationType { get; set; }
        public int RefreshTokenLifeTime { get; set; }
        public string Secret { get; set; }
        public bool IsActive { get; set; } = false;
        public bool UsePkce { get; set; }
        public IList<string> AllowedOrigins { get; set; }
        public IList<string?> GrantTypes { get; set; }
        public IList<string> AllowedScopes { get; set; }
        public string ClientUri { get; set; }
        public IList<string> RedirectUris { get; set; }
        /// <summary>
        /// Get or set the name of the clients/protected resource that are releated to this Client.
        /// </summary>
        public IList<string> AllowedProtectedResources { get; set; }
        #endregion

        #region Contructors

        public OAuthClientViewModel()
        {
        }

        public OAuthClientViewModel(MixCmsAccountContext context) : base(context)
        {
        }

        public OAuthClientViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public OAuthClientViewModel(OAuthClient entity, UnitOfWorkInfo uowInfo) : base(entity, uowInfo)
        {
        }
        #endregion
    }
}
