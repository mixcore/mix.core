using Mix.Constant.Enums;

namespace Mix.Service.Services
{
    public interface IEntityField : INamingField
    {
        public string Id { get; set; }
    }

    public class NamingField : INamingField
    {
        public NamingField(MixDatabaseNamingConvention namingConvention)
        {
            NamingConvention = namingConvention;
        }

        public MixDatabaseNamingConvention NamingConvention { get; set; }
    }

    public class EntityField : NamingField, IEntityField
    {
        public EntityField(MixDatabaseNamingConvention namingConvention) : base(namingConvention)
        {
            switch (namingConvention)
            {
                case MixDatabaseNamingConvention.TitleCase:
                    Id = "Id";
                    break;
                case MixDatabaseNamingConvention.SnakeCase:
                    Id = "id";
                    break;
            }
        }

        public string Id { get; set; }
    }

    public interface ICreationField
    {
        public string CreatedBy { get; set; }

        public string CreatedDateTime { get; set; }
    }

    public class CreationField : EntityField, ICreationField
    {
        public CreationField(MixDatabaseNamingConvention namingConvention) : base(namingConvention)
        {
            switch (namingConvention)
            {
                case MixDatabaseNamingConvention.TitleCase:
                    CreatedDateTime = "CreatedDateTime";
                    CreatedBy = "CreatedBy";
                    break;
                case MixDatabaseNamingConvention.SnakeCase:
                    CreatedDateTime = "created_date_time";
                    CreatedBy = "created_by";
                    break;
            }
        }

        public string CreatedBy { get; set; }

        public string CreatedDateTime { get; set; }
    }

    public interface IUpdatedField
    {
        public string ModifiedBy { get; set; }

        public string LastModified { get; set; }
    }

    public class UpdatedField : EntityField, IUpdatedField
    {
        public UpdatedField(MixDatabaseNamingConvention namingConvention) : base(namingConvention)
        {
            switch (namingConvention)
            {
                case MixDatabaseNamingConvention.TitleCase:
                    LastModified = "LastModified";
                    ModifiedBy = "ModifiedBy";
                    break;
                case MixDatabaseNamingConvention.SnakeCase:
                    LastModified = "last_modified";
                    ModifiedBy = "modified_by";
                    break;
            }
        }

        public string ModifiedBy { get; set; }

        public string LastModified { get; set; }
    }

    public interface IAuditField : IEntityField, ICreationField, IUpdatedField
    {
    }

    public class AuditField : EntityField, IAuditField
    {
        public AuditField(MixDatabaseNamingConvention namingConvention) : base(namingConvention)
        {
            switch (namingConvention)
            {
                case MixDatabaseNamingConvention.TitleCase:
                    CreatedDateTime = "CreatedDateTime";
                    LastModified = "LastModified";
                    CreatedBy = "CreatedBy";
                    ModifiedBy = "ModifiedBy";
                    break;
                case MixDatabaseNamingConvention.SnakeCase:
                    CreatedDateTime = "created_date_time";
                    LastModified = "last_modified";
                    CreatedBy = "created_by";
                    ModifiedBy = "modified_by";
                    break;
            }
        }

        public string CreatedBy { get; set; }

        public string CreatedDateTime { get; set; }

        public string ModifiedBy { get; set; }

        public string LastModified { get; set; }
    }

    public interface IRelationField : IAuditField
    {
        public string ParentId { get; set; }

        public string GuidParentId { get; set; }

        public string ParentType { get; set; }

        public string ParentDatabaseName { get; set; }

        public string ChildId { get; set; }

        public string GuidChildId { get; set; }

        public string ChildDatabaseName { get; set; }

        public string Priority { get; set; }

        public string Status { get; set; }

        public string IsDeleted { get; set; }

        public string TenantId { get; set; }
    }

    public interface IFieldNameService : IRelationField
    {
    }

    public interface INamingField
    {
        public MixDatabaseNamingConvention NamingConvention { get; set; }
    }

    public sealed class FieldNameService : NamingField, IFieldNameService
    {
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
        public FieldNameService(MixDatabaseNamingConvention namingConvention) : base(namingConvention)
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
                    TenantId = "TenantId";
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
                    TenantId = "tenant_id";
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
                if (item.PropertyType == typeof(string) && item.GetValue(this) != null)
                {
                    result.Add(item!.GetValue(this)!.ToString());
                }
            }
            return result;
        }
    }
}
