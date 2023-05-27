namespace Mix.Services.Ecommerce.Lib.Models.Onepay
{
    /*
     * Một giao dịch chỉ được coi là thành công nếu thỏa mãn đủ 2 điều kiện:
        - Tham số trả về vpc_TxnResponseCode = 0
        - Check giá trị hash đúng (hash validate)
        Trường hợp vpc_TxnResponsCode <> 0: tham chiếu bảng mã trả lời để hiển thị kết
        quả giao dịch cụ thể cho khách hàng.
     */
    public sealed class OnepayResponse
    {
        // 
        public string? vpc_Command { get; set; }
        // 
        public string? vpc_Locale { get; set; }
        // 
        public string? vpc_CurrencyCode { get; set; }
        // 
        public string? vpc_MerchTxnRef { get; set; }
        // 
        public string? vpc_Merchant { get; set; }
        // 
        public string? vpc_OrderInfo { get; set; }
        // 
        public string? vpc_Amount { get; set; }
        // 
        public string? vpc_TxnResponseCode { get; set; }
        // 
        public string? vpc_TransactionNo { get; set; }
        // 
        public string? vpc_Message { get; set; }
        // 
        public string? vpc_AdditionData { get; set; }
        // 
        public string? vpc_SecureHash { get; set; }

        public OnepayResponse()
        {

        }

    }
}
