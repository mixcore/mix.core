using Mix.Heart.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Database.Entities.Account
{
    public class Permission : EntityBase<int>
    {
        public int MixTenantId { get; set; }
        public string DisplayName { get; set; }
        public string Group { get; set; }
        public string Key { get; set; }
    }
}
