using Mix.Database.Entities.Account;
using Mix.Heart.Enums;
using Mix.Heart.Repository;
using Mix.Heart.Services;
using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;
using System;

namespace Mix.Identity.ViewModels
{
    public class RefreshTokenViewModel
        : ViewModelBase<MixCmsAccountContext, RefreshTokens, Guid, RefreshTokenViewModel>
    {
        public RefreshTokenViewModel()
        {
        }
        #region Contructors

        public RefreshTokenViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public RefreshTokenViewModel(
            RefreshTokens entity, 
            MixCacheService cacheService = null, 
            UnitOfWorkInfo uowInfo = null) 
            : base(entity, uowInfo)
        {
        }

        #endregion

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

        public override void InitDefaultValues(string language = null, int? cultureId = null)
        {
            Id = Guid.NewGuid();
            CreatedDateTime = DateTime.UtcNow;
            Status = MixContentStatus.Published;
        }

        #endregion
    }
}
