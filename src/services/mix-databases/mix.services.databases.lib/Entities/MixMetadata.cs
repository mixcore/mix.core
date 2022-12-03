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
        public MetadataParentType? Type { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public int MixTenantId { get; set; }
    }
}
