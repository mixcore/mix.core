using Microsoft.AspNetCore.Http;
using Mix.Service.Services;
using Mix.Shared.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Mix.Service.Models
{
    public class MixDbCommandModel
    {
        public int TenantId { get; set; }
        public string MixDbName { get; set; }
        public string ConnectionId { get; set; }
        public JObject Body { get; set; }
        public string? RequestedBy { get; set; }

        public MixDbCommandModel()
        {
        }
    }
}
