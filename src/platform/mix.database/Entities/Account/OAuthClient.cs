using Mix.Shared.Enums;
using System.Collections.Generic;

namespace Mix.Database.Entities.Account
{
    public partial class OAuthClient: EntityBase<Guid>
    {
        public string Name { get; set; }
        public ApplicationType ApplicationType { get; set; }
        public int RefreshTokenLifeTime { get; set; }
        public string Secret { get; set; }
        public bool IsActive { get; set; } = false;
        public bool UsePkce { get; set; }
        public IList<string> AllowedOrigins { get; set; }
        public IList<string> GrantTypes { get; set; }
        public IList<string> AllowedScopes { get; set; }
        public string ClientUri { get; set; }
        public IList<string> RedirectUris { get; set; }
        /// <summary>
        /// Get or set the name of the clients/protected resource that are releated to this Client.
        /// </summary>
        public IList<string> AllowedProtectedResources { get; set; }
    }
}