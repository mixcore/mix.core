using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;
using Mix.Services.Ecommerce.Lib.Entities.Paypal;
using Mix.Services.Ecommerce.Lib.Enums;

namespace Mix.Services.Ecommerce.Lib.ViewModels.Paypal
{
    public sealed class PaypalTransactionResponseViewModel
        : ViewModelBase<PaypalDbContext, PaypalTransactionResponse, int, PaypalTransactionResponseViewModel>
    {
        #region Properties
        public string vpc_Command { get; set; }
        public string vpc_Locale { get; set; }
        public string vpc_CurrencyCode { get; set; }
        public string vpc_MerchTxnRef { get; set; }
        public string vpc_Merchant { get; set; }
        public string vpc_OrderInfo { get; set; }
        public string vpc_Amount { get; set; }
        public string vpc_TxnResponseCode { get; set; }
        public string vpc_TransactionNo { get; set; }
        public string vpc_Message { get; set; }
        public string vpc_AdditionData { get; set; }
        public string vpc_SecureHash { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public int MixTenantId { get; set; }

        #endregion

        #region Contructors

        public PaypalTransactionResponseViewModel()
        {
        }

        public PaypalTransactionResponseViewModel(PaypalDbContext context) : base(context)
        {
        }

        public PaypalTransactionResponseViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public PaypalTransactionResponseViewModel(PaypalTransactionResponse entity, UnitOfWorkInfo? uowInfo = null) : base(entity, uowInfo)
        {
        }
        #endregion
    }
}
