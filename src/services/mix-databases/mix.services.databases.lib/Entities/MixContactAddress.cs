using Mix.Heart.Entities;
using Mix.Services.Databases.Lib.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Services.Databases.Lib.Entities
{
    public class MixContactAddress : EntityBase<int>
    {
        public string? Street { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Note { get; set; }
        public int SysUserDataId { get; set; }
        public int MixTenantId { get; set; }
    }
}
