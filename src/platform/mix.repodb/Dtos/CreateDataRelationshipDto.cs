using Mix.Constant.Enums;
using Newtonsoft.Json.Linq;

namespace Mix.RepoDb.Dtos
{
    public sealed class CreateDataRelationshipDto
    {
        public int MixTenantId { get; set; }
        public string ParentDatabaseName { get; set; }
        public string ChildDatabaseName { get; set; }
        public Guid? GuidParentId { get; set; }
        public Guid? GuidChildId { get; set; }
        public int ParentId { get; set; }
        public int ChildId { get; set; }

        public JObject ToJObject(MixDatabaseNamingConvention namingConvention)
        {
            switch (namingConvention)
            {
                case MixDatabaseNamingConvention.SnakeCase:
                    return new JObject()
                    {
                        new JProperty("mix_tenant_id", MixTenantId),
                        new JProperty("parent_database_name", ParentDatabaseName),
                        new JProperty("child_database_name", ChildDatabaseName),
                        new JProperty("guid_parent_id", GuidParentId),
                        new JProperty("guid_child_id", GuidChildId),
                        new JProperty("parent_id", ParentId),
                        new JProperty("child_id", ChildId)
                    };
                case MixDatabaseNamingConvention.TitleCase:
                default:
                    return JObject.FromObject(this);
            }
        }
    }
}
