namespace Mix.Shared.Models.Configurations
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
        public Guid ClientId { get; set; }
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Issuers { get; set; }
        public string Audiences { get; set; }
        public bool RequireUniqueEmail { get; set; }
        public bool RequireConfirmedEmail { get; set; }
        public string ConfirmedEmailUrl { get; set; }
        public string ConfirmedEmailUrlSuccess { get; set; }
        public string ConfirmedEmailUrlFail { get; set; }
        public int TokenLifespan { get; set; } = 3;
        public ExternalLogin Facebook { get; set; } = new ExternalLogin();
        public ExternalLogin Google { get; set; } = new ExternalLogin();
        public ExternalLogin Microsoft { get; set; } = new ExternalLogin();
        public ExternalLogin Twitter { get; set; } = new ExternalLogin();
        public AzureAdLogin AzureAd { get; set; } = new AzureAdLogin();
    }

    public class ExternalLogin
    {
        public string AppId { get; set; }
        public string AppSecret { get; set; }
    }
    public class AzureAdLogin
    {
        public string Instance { get; set; } = "https://login.microsoftonline.com/";
        public string ClientId { get; set; }
        public string TenantId { get; set; }
        public string Scopes { get; set; }
    }
}
