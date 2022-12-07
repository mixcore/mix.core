using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;
using Mix.Services.Ecommerce.Lib.Entities.Onepay;
using Mix.Services.Ecommerce.Lib.Enums;

namespace Mix.Services.Ecommerce.Lib.ViewModels.Onepay
{
    public sealed class OnepayTransactionRequestViewModel
        : ViewModelBase<OnepayDbContext, OnepayTransactionRequest, int, OnepayTransactionRequestViewModel>
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
        public PaymentStatus PaymentStatus { get; set; }
        public int MixTenantId { get; set; }

        #endregion

        #region Contructors

        public OnepayTransactionRequestViewModel()
        {
        }

        public OnepayTransactionRequestViewModel(OnepayDbContext context) : base(context)
        {
        }

        public OnepayTransactionRequestViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public OnepayTransactionRequestViewModel(OnepayTransactionRequest entity, UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
        {
        }
        #endregion
    }
}
