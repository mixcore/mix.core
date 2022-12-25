using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;
using Mix.Services.Ecommerce.Lib.Entities.Onepay;
using Mix.Services.Ecommerce.Lib.Enums;

namespace Mix.Services.Ecommerce.Lib.ViewModels.Onepay
{
    public sealed class OnepayTransactionResponseViewModel
        : ViewModelBase<OnepayDbContext, OnepayTransactionResponse, int, OnepayTransactionResponseViewModel>
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
        public OrderStatus PaymentStatus { get; set; }
        public int MixTenantId { get; set; }

        #endregion

        #region Contructors

        public OnepayTransactionResponseViewModel()
        {
        }

        public OnepayTransactionResponseViewModel(OnepayDbContext context) : base(context)
        {
        }

        public OnepayTransactionResponseViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public OnepayTransactionResponseViewModel(OnepayTransactionResponse entity, UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
        {
        }
        #endregion
    }
}
