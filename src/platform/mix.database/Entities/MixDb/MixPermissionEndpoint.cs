﻿namespace Mix.Database.Entities.MixDb
{
    public class MixPermissionEndpoint : EntityBase<int>
    {
        public string Title { get; set; }
        public string Path { get; set; }
        public string Method { get; set; }
        public string Description { get; set; }
        public int MixPermissionId { get; set; }
        public int TenantId { get; set; }
    }
}
