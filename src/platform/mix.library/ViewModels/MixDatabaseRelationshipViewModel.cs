using System.ComponentModel.DataAnnotations;

namespace Mix.Lib.ViewModels
{
    public class MixDatabaseRelationshipViewModel
        : AssociationViewModelBase<MixCmsContext, MixDatabaseRelationship, int, MixDatabaseRelationshipViewModel>
    {
        #region Properties
        public string DisplayName { get; set; }
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

        public override async Task Validate()
        {
            await base.Validate();
            if (ParentId == 0 || ChildId == 0)
            {
                IsValid = false;
                Errors.Add(new($"Ivalid relationship: leftId = {ParentId} - rightId = {ChildId} - Type = {Type}"));
            }
        }

        protected override async Task DeleteHandlerAsync()
        {
            var leftDb = Context.MixDatabase.Find(ParentId);
            string leftColName = $"{leftDb.SystemName}Id";
            var rightDb = Context.MixDatabase.Find(ChildId);
            string rightColName = $"{rightDb.SystemName}Id";
            await MixDatabaseColumnViewModel.GetRepository(UowInfo).DeleteManyAsync(
                m => (m.MixDatabaseId == ParentId && m.SystemName == rightColName)
                    || (m.MixDatabaseId == ChildId && m.SystemName == leftColName));
            await base.DeleteHandlerAsync();
        }

        #endregion
    }
}
