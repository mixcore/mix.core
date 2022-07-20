using System.ComponentModel.DataAnnotations;

namespace Mix.Lib.ViewModels
{
    [GenerateRestApiController]
    public class MixDatabaseRelationshipViewModel
        : AssociationViewModelBase<MixCmsContext, MixDatabaseRelationship, int, MixDatabaseRelationshipViewModel>
    {
        #region Properties
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
            if (Type == MixDatabaseRelationshipType.OneToMany || Type == MixDatabaseRelationshipType.ManyToMany)
            {
                if (!await colRepo.CheckIsExistsAsync(m => m.MixDatabaseId == RightId && m.SystemName == rightColName))
                {
                    var rightCol = new MixDatabaseColumnViewModel(UowInfo)
                    {
                        MixDatabaseId = rightDb.Id,
                        MixDatabaseName = rightDb.SystemName,
                        SystemName = rightColName,
                        DisplayName = rightColName,
                        DataType = MixDataType.Integer,
                        CreatedBy = CreatedBy
                    };
                    await rightCol.SaveAsync();
                }
            }
            if (Type == MixDatabaseRelationshipType.ManyToMany)
            {
                if (!await colRepo.CheckIsExistsAsync(m => m.MixDatabaseId == LeftId && m.SystemName == leftColName))
                {
                    var rightCol = new MixDatabaseColumnViewModel(UowInfo)
                    {
                        MixDatabaseId = leftDb.Id,
                        MixDatabaseName = leftDb.SystemName,
                        SystemName = leftColName,
                        DisplayName = leftColName,
                        DataType = MixDataType.Integer,
                        CreatedBy = CreatedBy
                    };
                    await rightCol.SaveAsync();
                }
            }
        }

        protected override async Task DeleteHandlerAsync()
        {
            await MixDataContentValueViewModel.GetRepository(UowInfo).DeleteManyAsync(m => m.MixDatabaseId == Id);
            await MixDataViewModel.GetRepository(UowInfo).DeleteManyAsync(m => m.MixDatabaseId == Id);
            await MixDatabaseColumnViewModel.GetRepository(UowInfo).DeleteManyAsync(m => m.MixDatabaseId == Id);
            await base.DeleteHandlerAsync();
        }

        #endregion
    }
}
