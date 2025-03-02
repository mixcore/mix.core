namespace Mix.Shared.Interfaces
{
    public interface IGlobalConfigurations
    {
        string DefaultDomain { get; set; }
        int? DefaultPageSize { get; set; }
        bool EnableOcelot { get; set; }
        bool IsUpdateSystemDatabases { get; set; }
        int ResponseCache { get; set; }
    }
}