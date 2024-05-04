﻿namespace Mix.Database.Entities.MixDb
{
    public class MixMetadata : EntityBase<int>
    {
        public string Type { get; set; }
        public string Content { get; set; }
        public string SeoContent { get; set; }
        public int MixTenantId { get; set; }
    }
}
