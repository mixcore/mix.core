using Mix.Services.Ecommerce.Lib.ViewModels;

namespace Mix.Services.Ecommerce.Lib.Models.Onepay
{
    public sealed class PaymentRequest
    {
        public string? vpc_AccessCode { get; set; }
        /*
         *  Khoản tiền thanh toán, không có dấu ngăn cách
            thập phân. thêm “00” trước khi chuyển sang
            cổng thanh toán.
            Nếu số tiền giao dịch là VND 25,000.00 thì số
            tiền gởi qua là : 2500000
         */
        public string? vpc_Amount { get; set; }
        public string? vpc_Command { get; set; } = "pay";
        public string? vpc_Currency { get; set; } = "VND";
        public string? vpc_Locale { get; set; } = "vn";
        public string? vpc_MerchTxnRef { get; set; }
        public string? vpc_Merchant { get; set; }
        
        public string? vpc_OrderInfo { get; set; }
        public string? vpc_ReturnURL { get; set; }
        public string? vpc_TicketNo { get; set; }
        public int? vpc_Version { get; set; } = 2;

        // Link trang thanh toán của website trước khi chuyển sang OnePAY
        public string? AgainLink { get; set; }
        // Tiêu đề cổng thanh toán hiển thị trên trình duyệt của chủ thẻ.
        public string? Title { get; set; }

        // Tham số chuỗi mã hóa, được mã hóa từ các tham số trên.
        // Chuỗi chữ ký đảm bảo toàn vẹn dữ liệu.
        public string? vpc_SecureHash { get; set; }

        // Thông tin khách hàng – Không bắt buộc
        public string? vpc_Customer_Phone { get; set; }
        public string? vpc_Customer_Email { get; set; }
        public string? vpc_Customer_Id { get; set; }

        public PaymentRequest()
        {

        }

        public PaymentRequest(OrderViewModel order)
        {
            vpc_Amount = $"{order.Total}00";
            vpc_Locale = "vn";
            vpc_MerchTxnRef = Guid.NewGuid().ToString();
            vpc_OrderInfo = order.Id.ToString();
            Title = order.Title;
        }
    }
}
