using Newtonsoft.Json.Linq;

namespace Mix.Shared.Models.Configurations
{
    public class GoogleSettingModel
    {
        public string ProjectId { get; set; }
        public string Filename { get; set; }
        public FirebaseSettingModel Firebase { get; set; }
        public GoogleStorageSettingModel Storage { get; set; }

    }
    public class FirebaseSettingModel
    {
        public string WebApiKey { get; set; }
        public Credential Credential { get; set; }
    }
    
    public class GoogleStorageSettingModel
    {
        public string BucketName { get; set; }
        public Credential? Credential { get; set; }
    }

    public class Credential
    {
        public string type { get; set; }
        public string project_id { get; set; }
        public string private_key_id { get; set; }
        public string private_key { get; set; }
        public string client_email { get; set; }
        public string client_id { get; set; }
        public string auth_uri { get; set; }
        public string token_uri { get; set; }
        public string auth_provider_x509_cert_url { get; set; }
        public string client_x509_cert_url { get; set; }
        public string universe_domain { get; set; }
    }
}
