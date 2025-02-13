using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;
using Mix.Services.Ecommerce.Lib.Entities.Paypal;
using Mix.Services.Ecommerce.Lib.Enums;
using Newtonsoft.Json.Linq;

namespace Mix.Services.Ecommerce.Lib.ViewModels.Paypal
{
    public sealed class PaypalTransactionResponseViewModel
        : ViewModelBase<PaypalDbContext, PaypalTransactionResponse, int, PaypalTransactionResponseViewModel>
    {
        #region Properties
        public string PaypalId { get; set; }
        public string PaypalStatus { get; set; }
        public JObject Payer { get; set; }
        public JObject PaymentSource { get; set; }
        public JArray PurchaseUnits { get; set; }
        public JArray Links { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public int TentantId { get; set; }
        public string Intent { get; set; }
        public string State { get; set; }
        public string Cart { get; set; }
        public JArray Transactions { get; set; }
        public DateTime CreatedTime { get; set; }
        public int TenantId { get; set; }

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
