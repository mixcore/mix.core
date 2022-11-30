namespace Mix.Services.Payments.Lib.ViewModels.Onepay.Onepay
{
    public sealed class PaymentQueryResponse
    {
        /*
         * Xác định giao dịch tồn tại hay không
            ✓ N: Không tồn tại giao dịch
            ✓ Y: Có tồn tại giao dịch thanh toán
         */
        public string vpc_DRExists { get; set; }

        /*
         * Mã trả lời, xác định giao dịch thành công hay không
            ✓ 0: Giao dịch thanh toán thành công
            Trang 7 / 10
            ✓ <> 0: Giao dịch không thanh toán thành công
            ✓ 300: Giao dịch pending
            ✓ 100: Giao dịch đang tiến hành hoặc chưa thanh toán         
         */
        public string vpc_TxnResponseCode { get; set; }
    }
}
