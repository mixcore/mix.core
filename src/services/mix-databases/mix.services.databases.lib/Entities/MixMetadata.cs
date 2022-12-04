using Mix.Heart.Entities;
using Mix.Services.Databases.Lib.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Services.Databases.Lib.Entities
{
    public class MixMetadata: EntityBase<int>
    {
        public string? Type { get; set; }
        public string Content { get; set; }
        public string SeoContent { get; set; }
        public int MixTenantId { get; set; }
    }
}
