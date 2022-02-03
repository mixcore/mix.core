namespace Mix.Identity.Models
{
    public class MixAuthenticationConfigurations
    {
        public int AccessTokenExpiration { get; set; } = 20;
        public int RefreshTokenExpiration { get; set; } = 20;
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
        public ExternalLogin Facebook { get; set; } = new ExternalLogin();
        public ExternalLogin Google { get; set; } = new ExternalLogin();
        public ExternalLogin Microsoft { get; set; } = new ExternalLogin();
        public ExternalLogin Twitter { get; set; } = new ExternalLogin();
    }

    public class ExternalLogin
    {
        public string AppId { get; set; }
        public string AppSecret { get; set; }
    }
}
