namespace Mix.Storage.Lib.Engines.CloudFlare
{
    public class CloudFlareSettings
    {
        public string EndpointTemplate { get; set; }
        public string ApiToken { get; set; }
        public string ZoneId { get; set; }
        public string AccountId { get; set; }
        public CloudFlareApiKey ApiKey { get; set; }
    }
    public class CloudFlareApiKey
    {
        public string GlobalKey { get; set; }
        public string OriginKey { get; set; }
    }
}
