﻿using Mix.Heart.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Universal.Lib.Entities
{
    public class Organization: EntityBase<int>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Endpoint { get; set; }
        public int? TenantId { get; set; }
    }
}