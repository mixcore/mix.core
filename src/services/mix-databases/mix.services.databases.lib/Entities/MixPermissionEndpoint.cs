﻿using Mix.Heart.Entities;

namespace Mix.Services.Databases.Lib.Entities
{
    public class MixPermissionEndpoint : EntityBase<int>
    {
        public string Title { get; set; }
        public string Path { get; set; }
        public string Method { get; set; }
        public string Description { get; set; }
        public int SysPermissionId { get; set; }
        public int MixTenantId { get; set; }
    }
}