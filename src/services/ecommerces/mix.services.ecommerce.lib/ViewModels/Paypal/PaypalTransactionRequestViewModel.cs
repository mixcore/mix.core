using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;
using Mix.Services.Ecommerce.Lib.Entities.Paypal;
using Mix.Services.Ecommerce.Lib.Enums;

namespace Mix.Services.Ecommerce.Lib.ViewModels.Paypal
{
    public sealed class PaypalTransactionRequestViewModel
        : ViewModelBase<PaypalDbContext, PaypalTransactionRequest, int, PaypalTransactionRequestViewModel>
    {
        #region Properties
        public int vpc_Version { get; set; }
        public string vpc_Currency { get; set; }
        public string vpc_Command { get; set; }
        public string vpc_AccessCode { get; set; }
        public string vpc_Merchant { get; set; }
        public string vpc_Locale { get; set; }
        public string vpc_ReturnURL { get; set; }
        public string vpc_MerchTxnRef { get; set; }
        public string vpc_OrderInfo { get; set; }
        public string vpc_Amount { get; set; }
        public string vpc_TicketNo { get; set; }
        public string AgainLink { get; set; }
        public string Title { get; set; }
        public string vpc_SecureHash { get; set; }
        public string vpc_Customer_Phone { get; set; }
        public string vpc_Customer_Email { get; set; }
        public string vpc_Customer_Id { get; set; }
        public int? ResponseId { get; set; }
        public OrderStatus PaymentStatus { get; set; }
        public int MixTenantId { get; set; }

        #endregion

        #region Contructors

        public PaypalTransactionRequestViewModel()
        {
        }

        public PaypalTransactionRequestViewModel(PaypalDbContext context) : base(context)
        {
        }

        public PaypalTransactionRequestViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public PaypalTransactionRequestViewModel(PaypalTransactionRequest entity, UnitOfWorkInfo? uowInfo = null) : base(entity, uowInfo)
        {
        }
        #endregion
    }
}
