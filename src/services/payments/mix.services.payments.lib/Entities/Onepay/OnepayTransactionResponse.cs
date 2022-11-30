using Mix.Heart.Entities;
using Mix.Services.Payments.Lib.Enums;

namespace Mix.Services.Payments.Lib.Entities.Onepay
{
    public class OnepayTransactionResponse : EntityBase<int>
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
        public PaymentStatus OnepayStatus { get; set; }
    }
}
