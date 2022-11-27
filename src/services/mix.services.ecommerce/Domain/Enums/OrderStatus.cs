namespace mix.services.ecommerce.Domain.Enums
{
    public enum OrderStatus
    {
        New,
        WaitForPayment,
        PaymentSuccess,
        PaymentFailure
    }
}
