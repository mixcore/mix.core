using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Shared.Models
{
    public sealed class MixDbAuditLogModel
    {
        public object Id{ get; set; }
        public string MixDbName{ get; set; }
        public JObject Before { get; set; }
        public JObject After { get; set; }
        public JObject Body { get; set; }
    }
}
