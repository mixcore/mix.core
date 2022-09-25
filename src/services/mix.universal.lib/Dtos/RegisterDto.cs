using Mix.Identity.Constants;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Universal.Lib.Dtos
{
    public class RegisterDto
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

        public UserInformationDto Information { get; set; }
    }

    public class UserInformationDto
    {
        public int Avatar { get; set; }
        public UserOrganizationDto Organization { get; set; }
    }

    public class UserOrganizationDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Endpoint { get; set; }
    }
}
