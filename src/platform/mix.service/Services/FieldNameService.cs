using Mix.Constant.Enums;

namespace Mix.Service.Services
{
    public sealed class FieldNameService
    {
        public MixDatabaseNamingConvention NamingConvention { get; set; }
        #region Properties
        public string Id { get; set; }
        public string ParentId { get; set; }
        public string GuidParentId { get; set; }
        public string ParentType { get; set; }
        public string ParentDatabaseName { get; set; }
        public string ChildId { get; set; }
        public string GuidChildId { get; set; }
        public string ChildDatabaseName { get; set; }
        public string CreatedDateTime { get; set; }
        public string LastModified { get; set; }
        public string TenantId { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public string IsDeleted { get; set; }
        #endregion
        public FieldNameService(MixDatabaseNamingConvention namingConvention)
        {
            NamingConvention = namingConvention;
            switch (namingConvention)
            {
                case MixDatabaseNamingConvention.TitleCase:
                    Id = "Id";
                    ParentId = "ParentId";
                    GuidParentId = "GuidParentId";
                    ParentType = "ParentType";
                    ParentDatabaseName = "ParentDatabaseName";
                    ChildId = "ChildId";
                    GuidChildId = "GuidChildId";
                    ChildDatabaseName = "ChildDatabaseName";
                    CreatedDateTime = "CreatedDateTime";
                    LastModified = "LastModified";
                    TenantId = "MixTenantId";
                    CreatedBy = "CreatedBy";
                    ModifiedBy = "ModifiedBy";
                    Priority = "Priority";
                    Status = "Status";
                    IsDeleted = "IsDeleted";
                    break;
                case MixDatabaseNamingConvention.SnakeCase:
                    Id = "id";
                    ParentId = "parent_id";
                    GuidParentId = "guid_parent_id";
                    ParentType = "parent_type";
                    ParentDatabaseName = "parent_database_name";
                    ChildId = "child_id";
                    GuidChildId = "guid_child_id";
                    ChildDatabaseName = "child_database_name";
                    CreatedDateTime = "created_date_time";
                    LastModified = "last_modified";
                    TenantId = "mix_tenant_id";
                    CreatedBy = "created_by";
                    ModifiedBy = "modified_by";
                    Priority = "priority";
                    Status = "status";
                    IsDeleted = "is_deleted";
                    break;
            }
        }

        public string GetParentId(string parentName)
        {
            switch (NamingConvention)
            {
                case MixDatabaseNamingConvention.SnakeCase:
                    return $"{parentName}_id";
                case MixDatabaseNamingConvention.TitleCase:
                default:
                    return $"{parentName}Id";
            }
        }

        public List<string> GetAllFieldName()
        {
            List<string> result = new();
            var props = GetType().GetProperties();
            foreach (var item in props)
            {
                result.Append(item.GetValue(this)?.ToString());
            }
            return result;
        }
    }
}
