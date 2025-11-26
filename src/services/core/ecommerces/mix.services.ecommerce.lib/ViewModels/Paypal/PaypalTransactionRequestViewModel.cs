using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;
using Mix.Services.Ecommerce.Lib.Entities.Paypal;
using Mix.Services.Ecommerce.Lib.Enums;
using Mix.Services.Ecommerce.Lib.Models.Paypal;
using Newtonsoft.Json.Linq;

namespace Mix.Services.Ecommerce.Lib.ViewModels.Paypal
{
    public sealed class PaypalTransactionRequestViewModel
        : ViewModelBase<PaypalDbContext, PaypalTransactionRequest, int, PaypalTransactionRequestViewModel>
    {
        #region Properties
        public string? Intent { get; set; }
        public JObject Payer { get; set; }
        public JObject RedirectUrls { get; set; }
        public JArray Transactions { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public int TenantId { get; set; }

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
