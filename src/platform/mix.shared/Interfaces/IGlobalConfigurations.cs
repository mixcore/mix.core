namespace Mix.Shared.Interfaces
{
    public interface IGlobalConfigurations
    {
        bool AllowAnyOrigin { get; set; }
        string ApiEncryptKey { get; set; }
        string DefaultDomain { get; set; }
        bool EnableAuditLog { get; set; }
        bool EnableOcelot { get; set; }
        InitStep InitStatus { get; set; }
        bool IsHttps { get; set; }
        bool IsInit { get; set; }
        bool IsLogStream { get; set; }
        bool MigrateSystemDatabases { get; set; }
        int ResponseCache { get; set; }
    }
}