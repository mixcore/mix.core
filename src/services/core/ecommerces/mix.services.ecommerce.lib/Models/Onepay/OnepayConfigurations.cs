namespace Mix.Services.Ecommerce.Lib.Models.Onepay
{
    public sealed class OnepayConfigurations
    {
        public bool Enabled { get; set; }
        public int Version { get; set; }
        public string Currency { get; set; }
        public string PaymentEndpoint { get; set; }
        public string Endpoint { get; set; }
        public string SecureHashKey { get; set; }
        public string AccessCode { get; set; }
        public string Merchant { get; set; }
        public string Locale { get; set; }
    }
}
