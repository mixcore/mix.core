namespace Mix.Services.Ecommerce.Lib.Enums
{
    public enum OrderStatus
    {
        NEW,
        WAITING_FOR_PAYMENT,
        PAID,
        SHIPPING,
        SUCCESS,
        PAYMENT_FAILED,
        SHIPPING_FAILED
    }
}
