using Mix.Auth.Enums;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;

namespace Mix.Auth.Models
{
    public sealed class RegisterRequestModel
    {
        [Required]
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public MixExternalLoginProviders? Provider { get; set; }
        public string ProviderKey { get; set; }
        public string Password { get; set; }
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public JObject Data { get; set; }
    }
}
