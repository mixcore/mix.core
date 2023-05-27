using Mix.Heart.Entities;
using Mix.Services.Ecommerce.Lib.Enums;

namespace Mix.Services.Ecommerce.Lib.Entities.Paypal
{
    public class PaypalTransactionResponse : EntityBase<int>
    {
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
        public OrderStatus PaypalStatus { get; set; }
    }
}
