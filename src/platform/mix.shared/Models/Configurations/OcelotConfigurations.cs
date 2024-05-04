namespace Mix.Shared.Models.Configurations
{
    public class OcelotConfigurations
    {
        public List<OcelotRoute> Routes { get; set; }
        public OcelotGlobalConfiguration GlobalConfiguration { get; set; }
    }

    public class OcelotGlobalConfiguration
    {
        public OcelotRateLimitOptions RateLimitOptions { get; set; }
        public OcelotHttpHandlerOptions HttpHandlerOptions { get; set; }
        public string BaseUrl { get; set; }
    }

    public class OcelotRoute
    {
        public int Priority { get; set; }
        public string DownstreamPathTemplate { get; set; }
        public string DownstreamScheme { get; set; }
        public OcelotHost DownstreamHostAndPorts { get; set; }
        public string UpstreamPathTemplate { get; set; }
        public string[] UpstreamHttpMethod { get; set; }
        public OcelotRateLimitOptions RateLimitOptions { get; set; }
        public OcelotFileCacheOptions FileCacheOptions { get; set; }
        public OcelotHttpHandlerOptions HttpHandlerOptions { get; set; }
    }

    public class OcelotHttpHandlerOptions
    {
        public bool AllowAutoRedirect { get; set; }
        public bool UseCookieContainer { get; set; }
        public bool UseTracing { get; set; }
        public int MaxConnectionsPerServer { get; set; }
    }

    public class OcelotFileCacheOptions
    {
        public int TtlSeconds { get; set; }
        public string Region { get; set; }
    }

    public class OcelotRateLimitOptions
    {
        public string[] ClientWhitelist { get; set; }
        public bool EnableRateLimiting { get; set; }
        public string Period { get; set; }
        public int PeriodTimespan { get; set; }
        public int Limit { get; set; }
    }

    public class OcelotHost
    {
        public string Host { get; set; }
        public int Port { get; set; }
    }
}
