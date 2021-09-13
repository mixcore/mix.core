namespace Mix.Cms.Payment.Domain.Dtos
{
    public class GetPayPalTokenDto
    {
        public string grant_type { get; set; } = "client_credentials";
    }
}
