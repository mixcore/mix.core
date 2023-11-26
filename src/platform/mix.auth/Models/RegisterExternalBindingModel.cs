using Mix.Auth.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
