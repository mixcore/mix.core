using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Mix.Lib.ViewModels
{
    public sealed class MixDbTableRelationshipViewModel
        : ViewModelBase<MixCmsContext, MixDbTableRelationship, int, MixDbTableRelationshipViewModel>
    {
        #region Properties
        public int ParentId { get; set; }
        public int ChildId { get; set; }
        public string DisplayName { get; set; }
        public string SourceDatabaseName { get; set; }
        public string DestinateDatabaseName { get; set; }
        public MixDbTableRelationshipType Type { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public string ReferenceColumnName => $"{SourceDatabaseName.ToTitleCase()}Id";
        #endregion

        #region Constructors

        public MixDbTableRelationshipViewModel()
        {

        }

        public MixDbTableRelationshipViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public MixDbTableRelationshipViewModel(MixDbTableRelationship entity, UnitOfWorkInfo uowInfo)
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

        //protected override async Task SaveEntityRelationshipAsync(MixDbTableRelationship parentEntity, CancellationToken cancellationToken = default)
        //{
        //    if (!Context.MixDbColumn.Any(m => m.MixDbTableName == DestinateTableName && m.SystemName == ReferenceColumnName))
        //    {
        //        var srcDb = Context.MixDbTableName.FirstOrDefault(m => m.SystemName == SourceTableName);
        //        var destDb = Context.MixDbTableName.FirstOrDefault(m => m.SystemName == DestinateTableName);
        //        var refCol = new MixDbColumnViewModel(UowInfo)
        //        {
        //            MixDbTableName = DestinateTableName,
        //            MixDbDatabaseId = destDb.Id,
        //            DataType = MixDataType.Reference,
        //            CreatedBy = CreatedBy,
        //            DisplayName = ReferenceColumnName.ToTitleCase(),
        //            SystemName = ReferenceColumnName
        //        };

        //        await refCol.SaveAsync(cancellationToken);
        //        ModifiedEntities.AddRange(refCol.ModifiedEntities);
        //    }
        //}

        #endregion
    }
}
