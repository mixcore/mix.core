namespace Mix.Identity.Models
{
    public class MixAuthenticationConfigurations
    {
        public int CookieExpiration { get; set; }
        public int RefreshTokenExpiration { get; set; }
        public bool ValidateIssuer { get; set; }
        public bool ValidateAudience { get; set; }
        public bool ValidateLifetime { get; set; }
        public bool ValidateIssuerSigningKey { get; set; }
        public string TokenType { get; set; }
        public string Audience { get; set; }
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Issuers { get; set; }
        public string Audiences { get; set; }
        public ExternalLogin Facebook { get; set; }
        public ExternalLogin Google { get; set; }
        public ExternalLogin Microsoft { get; set; }
        public ExternalLogin Twitter { get; set; }
    }

    public class ExternalLogin
    {
        public string AppId { get; set; }
        public string AppSecret { get; set; }
    }
}
