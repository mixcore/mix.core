using Mix.Auth.Enums;

namespace Mix.Auth.Models
{
    public class ExternalLoginViewModel
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public string State { get; set; }

        public MixExternalLoginProviders Provider { get; set; }
    }
}
