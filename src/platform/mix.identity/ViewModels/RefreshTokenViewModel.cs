using Mix.Database.Entities.Account;
using System;

namespace Mix.Identity.ViewModels
{
    public class RefreshTokenViewModel
        : ViewModelBase<MixCmsAccountContext, RefreshTokens, Guid, RefreshTokenViewModel>
    {
        public RefreshTokenViewModel()
        {
        }
        #region Constructors

        public RefreshTokenViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public RefreshTokenViewModel(
            RefreshTokens entity,

            UnitOfWorkInfo uowInfo = null)
            : base(entity, uowInfo)
        {
        }

        #endregion

        #region Properties

        #region Models

        public Guid ClientId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public DateTime ExpiresUtc { get; set; }
        public DateTime IssuedUtc { get; set; }

        #endregion Models

        #endregion Properties

        #region Overrides

        public override void InitDefaultValues(string language = null, int? cultureId = null)
        {
            Id = Guid.NewGuid();
            CreatedDateTime = DateTime.UtcNow;
            Status = MixContentStatus.Published;
        }

        #endregion
    }
}
