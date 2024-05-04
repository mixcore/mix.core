namespace Mix.Services.Ecommerce.Lib.Models.Paypal
{
    public sealed class PayPalAccessToken
    {
        public string scope { get; set; }
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string app_id { get; set; }
        public int expires_in { get; set; }
        public DateTime? expires_at { get => DateTime.Now.AddTicks(expires_in); }
        public string nonce { get; set; }
    }
}
