using System.ComponentModel.DataAnnotations;

namespace Mix.Lib.ViewModels
{
    [GenerateRestApiController]
    public class MixDatabaseRelationshipViewModel
        : AssociationViewModelBase<MixCmsContext, MixDatabaseRelationship, int, MixDatabaseRelationshipViewModel>
    {
        #region Properties
        public string SourceDatabaseName { get; set; }
        public string DestinateDatabaseName { get; set; }
        public MixDatabaseRelationshipType Type { get; set; }
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
        public override async Task ExpandView()
        {
        }

        public override async Task Validate()
        {
            await base.Validate();
            if (LeftId == 0 || RightId == 0)
            {
                IsValid = false;
                Errors.Add(new($"Ivalid relationship: leftId = {LeftId} - rightId = {RightId} - Type = {Type}"));
            }
        }

        protected override async Task SaveEntityRelationshipAsync(MixDatabaseRelationship parentEntity)
        {
            var colRepo = MixDatabaseColumnViewModel.GetRepository(UowInfo);
            // Add Reference column to right db
            var leftDb = Context.MixDatabase.Find(LeftId);
            string leftColName = $"{leftDb.SystemName}Id";
            var rightDb = Context.MixDatabase.Find(RightId);
            string rightColName = $"{rightDb.SystemName}Id";
            if (Type == MixDatabaseRelationshipType.OneToMany)
            {
                if (!await colRepo.CheckIsExistsAsync(m => m.MixDatabaseId == RightId && m.SystemName == leftColName))
                {
                    var col = new MixDatabaseColumnViewModel(UowInfo)
                    {
                        MixDatabaseId = rightDb.Id,
                        MixDatabaseName = rightDb.SystemName,
                        ReferenceId = leftDb.Id,
                        SystemName = leftColName,
                        DisplayName = leftColName,
                        DataType = MixDataType.Reference,
                        CreatedBy = CreatedBy
                    };
                    await col.SaveAsync();
                }
            }
            if (Type == MixDatabaseRelationshipType.ManyToMany)
            {
                if (!await colRepo.CheckIsExistsAsync(m => m.MixDatabaseId == RightId && m.SystemName == leftColName))
                {
                    var col = new MixDatabaseColumnViewModel(UowInfo)
                    {
                        MixDatabaseId = rightDb.Id,
                        MixDatabaseName = rightDb.SystemName,
                        ReferenceId = leftDb.Id,
                        SystemName = leftColName,
                        DisplayName = leftColName,
                        DataType = MixDataType.Reference,
                        CreatedBy = CreatedBy
                    };
                    await col.SaveAsync();
                }

                if (!await colRepo.CheckIsExistsAsync(m => m.MixDatabaseId == LeftId && m.SystemName == rightColName))
                {
                    var leftCol = new MixDatabaseColumnViewModel(UowInfo)
                    {
                        MixDatabaseId = leftDb.Id,
                        MixDatabaseName = leftDb.SystemName,
                        ReferenceId = rightDb.Id,
                        SystemName = rightColName,
                        DisplayName = rightColName,
                        DataType = MixDataType.Reference,
                        CreatedBy = CreatedBy
                    };
                    await leftCol.SaveAsync();
                }
            }
        }

        protected override async Task DeleteHandlerAsync()
        {
            var leftDb = Context.MixDatabase.Find(LeftId);
            string leftColName = $"{leftDb.SystemName}Id";
            var rightDb = Context.MixDatabase.Find(RightId);
            string rightColName = $"{rightDb.SystemName}Id";
            await MixDatabaseColumnViewModel.GetRepository(UowInfo).DeleteManyAsync(
                m => (m.MixDatabaseId == LeftId && m.SystemName == rightColName)
                    || (m.MixDatabaseId == RightId && m.SystemName == leftColName));
            await base.DeleteHandlerAsync();
        }

        #endregion
    }
}
