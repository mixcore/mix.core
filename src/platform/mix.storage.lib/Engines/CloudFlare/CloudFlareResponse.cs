namespace Mix.Storage.Lib.Engines.CloudFlare
{
    public class CloudFlareResponse
    {
        public CloudFlareResult Result { get; set; }
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
        public List<string> Messages { get; set; }
    }
    public class CloudFlareResult
    {
        public string Id { get; set; }
        public string FileName { get; set; }
        public DateTime Uploaded { get; set; }
        public bool RequireSignedURLs { get; set; }
        public List<string> Variants { get; set; }
    }
}
