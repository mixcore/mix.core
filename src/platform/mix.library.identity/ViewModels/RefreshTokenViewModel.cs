using Mix.Database.Entities.Account;
using Mix.Heart.Repository;
using Mix.Heart.ViewModel;
using System;

namespace Mix.Identity.ViewModels
{
    public class RefreshTokenViewModel
        : ViewModelBase<MixCmsAccountContext, RefreshTokens, Guid>
    {
        public RefreshTokenViewModel(CommandRepository<MixCmsAccountContext, RefreshTokens, Guid> repository) : base(repository)
        {
        }

        #region Properties

        #region Models

        public string ClientId { get; set; }

        public string Email { get; set; }

        public string Username { get; set; }

        public DateTime ExpiresUtc { get; set; }

        public DateTime IssuedUtc { get; set; }

        #endregion Models

        #endregion Properties
    }
}
