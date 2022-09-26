using Mix.Heart.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Universal.Lib.Entities
{
    public class PortalApp : EntityBase<int>
    {
        public string Title { get; set; }
        public string Path { get; set; }
        public string MixcoreVersion { get; set; }
        public int OrganizationId { get; set; }
        public int? TenantId { get; set; }
    }
}
