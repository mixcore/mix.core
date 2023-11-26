using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Auth.Models
{
    public class ExternalLoginResultModel
    {
        public string Token { get; set; }
        public string ReturnUrl { get; set; }
    }
}
