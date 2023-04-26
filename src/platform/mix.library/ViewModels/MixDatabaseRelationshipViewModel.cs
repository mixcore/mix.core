using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Mix.Lib.ViewModels
{
    public sealed class MixDatabaseRelationshipViewModel
        : ViewModelBase<MixCmsContext, MixDatabaseRelationship, int, MixDatabaseRelationshipViewModel>
    {
        #region Properties
        public int ParentId { get; set; }
        public int ChildId { get; set; }
        public string DisplayName { get; set; }
        public string SourceDatabaseName { get; set; }
        public string DestinateDatabaseName { get; set; }
        public MixDatabaseRelationshipType Type { get; set; }

        [JsonIgnore]
        public string ReferenceColumnName => $"{SourceDatabaseName.ToTitleCase()}Id";
        #endregion

        #region Constructors

        public MixDatabaseRelationshipViewModel()
        {

        }

        public MixDatabaseRelationshipViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public MixDatabaseRelationshipViewModel(MixDatabaseRelationship entity, UnitOfWorkInfo uowInfo = null)
            : base(entity, uowInfo)
        {
        }

        #endregion

        #region Overrides
        public override async Task Validate(CancellationToken cancellationToken)
        {
            if (Repository.Table.Any(m => !m.Id.Equals(Id) && m.ParentId.Equals(ParentId) && m.ChildId.Equals(ChildId)))
            {
                IsValid = false;
                Errors.Add(new ValidationResult("Entity Existed"));
            }
            if (MixHelper.IsDefaultId(ParentId))
            {
                IsValid = false;
                Errors.Add(new("Parent Id cannot be null"));
            }
            if (MixHelper.IsDefaultId(ChildId))
            {
                IsValid = false;
                Errors.Add(new("Child Id cannot be null"));
            }
            if (ParentId == 0 || ChildId == 0)
            {
                IsValid = false;
                Errors.Add(new($"Ivalid relationship: parent Id = {ParentId} - child Id = {ChildId} - Type = {Type}"));
            }

            await base.Validate(cancellationToken);
        }

        protected override async Task SaveEntityRelationshipAsync(MixDatabaseRelationship parentEntity, CancellationToken cancellationToken = default)
        {
            if (!Context.MixDatabaseColumn.Any(m => m.MixDatabaseName == DestinateDatabaseName && m.SystemName == ReferenceColumnName))
            {
                var srcDb = Context.MixDatabase.FirstOrDefault(m => m.SystemName == SourceDatabaseName);
                var destDb = Context.MixDatabase.FirstOrDefault(m => m.SystemName == DestinateDatabaseName);
                var refCol = new MixDatabaseColumnViewModel(UowInfo)
                {
                    MixDatabaseName = DestinateDatabaseName,
                    MixDatabaseId = destDb.Id,
                    DataType = MixDataType.Reference,
                    CreatedBy = CreatedBy,
                    DisplayName = ReferenceColumnName.ToTitleCase(),
                    SystemName = ReferenceColumnName
                };

                await refCol.SaveAsync(cancellationToken);
            }
        }

        #endregion
    }
}
