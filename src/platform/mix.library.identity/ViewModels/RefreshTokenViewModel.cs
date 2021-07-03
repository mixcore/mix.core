using Mix.Database.Entities.Account;
using Mix.Heart.Enums;
using Mix.Heart.Repository;
using Mix.Heart.ViewModel;
using System;

namespace Mix.Identity.ViewModels
{
    public class RefreshTokenViewModel
        : ViewModelBase<MixCmsAccountContext, RefreshTokens, Guid>
    {
        public RefreshTokenViewModel(MixCmsAccountContext context) : base(context)
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

        #region Overrides

        protected override void InitEntityValues()
        {
            if (Id == default)
            {
                Id = Guid.NewGuid();
                CreatedDateTime = DateTime.UtcNow;
                Status = MixContentStatus.Published;
            }
        }

        #endregion
    }
}
