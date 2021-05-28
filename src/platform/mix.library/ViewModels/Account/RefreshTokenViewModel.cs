using Microsoft.EntityFrameworkCore.Storage;
using Mix.Heart.Infrastructure.ViewModels;
using Newtonsoft.Json;
using System;
using Mix.Lib.Entities.Account;

namespace Mix.Lib.ViewModels.Account
{
    public class RefreshTokenViewModel
        : ViewModelBase<MixCmsAccountContext, RefreshTokens, RefreshTokenViewModel>
    {
        #region Properties

        #region Models

        public string Id { get; set; }

        public string ClientId { get; set; }

        public string Email { get; set; }

        public string Username { get; set; }

        public DateTime ExpiresUtc { get; set; }

        public DateTime IssuedUtc { get; set; }

        #endregion Models

        #endregion Properties

        #region Contructors

        public RefreshTokenViewModel() : base()
        {
            IsCache = false;
        }

        public RefreshTokenViewModel(RefreshTokens model, MixCmsAccountContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
            IsCache = false;
        }

        #endregion Contructors
    }
}