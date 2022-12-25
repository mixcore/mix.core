using Mix.Services.Ecommerce.Lib.ViewModels;

namespace Mix.Services.Ecommerce.Lib.Models.Onepay
{
    public sealed class PaymentRequest
    {
        // Version module cổng thanh toán, mặc định là “2”
        public int? vpc_Version { get; set; } = 2;
        // Loại tiền thanh toán, mặc định là VND
        public string? vpc_Currency { get; set; } = "VND";
        // Chức năng thanh toán, giá trị của đối số này mặc định là “pay”
        public string? vpc_Command { get; set; } = "pay";

        //// Cặp giá trị của mỗi đơn vị do OnePAY cấp
        public string? vpc_AccessCode { get; set; }
        public string? vpc_Merchant { get; set; }
        // Ngôn ngữ hiển thị khi thanh toán. Tiếng Việt:vn, tiếng Anh: en
        public string? vpc_Locale { get; set; } = "vn";

        /*
         *  Khoản tiền thanh toán, không có dấu ngăn cách
            thập phân. thêm “00” trước khi chuyển sang
            cổng thanh toán.
            Nếu số tiền giao dịch là VND 25,000.00 thì số
            tiền gởi qua là : 2500000
         */
        public string? vpc_Amount { get; set; }
        // Mã giao dịch, biến số này yêu cầu là duy nhất mỗi lần gửi sang OnePAY
        public string? vpc_MerchTxnRef { get; set; }
        // Thông tin đơn hàng, thường là mã đơn hàng hoặc mô tả ngắn gọn về đơn hàng
        public string? vpc_OrderInfo { get; set; }

        // URL Website ĐVCNTT để nhận kết quả trả về.
        public string? vpc_ReturnURL { get; set; }
        // Địa chỉ IP khách hàng thực hiện thanh toán – Không được đặt cố định 1 IP
        public string? vpc_TicketNo { get; set; }
        

        // Các tham số website gán giá trị động: Price, Order ID...



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
