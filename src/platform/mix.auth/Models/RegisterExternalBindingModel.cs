using Mix.Auth.Enums;

namespace Mix.Auth.Models
{
    public class RegisterExternalBindingModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public MixExternalLoginProviders Provider { get; set; }

        public string ExternalAccessToken { get; set; }

    }
}
