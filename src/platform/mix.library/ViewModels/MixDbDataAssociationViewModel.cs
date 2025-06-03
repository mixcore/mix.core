using System.Linq.Expressions;

namespace Mix.Lib.ViewModels
{
    public sealed class MixDbDataAssociationViewModel
        : ViewModelBase<MixCmsContext, MixDbDataAssociation, int, MixDbDataAssociationViewModel>
    {
        #region Properties
        public int TenantId { get; set; }
        public string ParentDatabaseName { get; set; }
        public string ChildDatabaseName { get; set; }
        public Guid? GuidParentId { get; set; }
        public Guid? GuidChildId { get; set; }
        public int ParentId { get; set; }
        public int ChildId { get; set; }
        #endregion

        #region Constructors

        public MixDbDataAssociationViewModel()
        {
        }

        public MixDbDataAssociationViewModel(MixDbDataAssociation entity, UnitOfWorkInfo uowInfo)
            : base(entity, uowInfo)
        {
        }

        public MixDbDataAssociationViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        #endregion

        #region Overrides

        public override async Task Validate(CancellationToken cancellationToken)
        {
            await base.Validate(cancellationToken);
            if (ParentId == 0 && !GuidParentId.HasValue)
            {
                IsValid = false;
                Errors.Add(new("Invalid Parent"));
            }

            Expression<Func<MixDbDataAssociation, bool>> predicate = m =>
                        m.Id != Id
                        && m.ParentDatabaseName == ParentDatabaseName
                        && m.ChildDatabaseName == ChildDatabaseName
                        && m.ChildId == ChildId;
            predicate = predicate.AndAlsoIf(ParentId > 0, m => m.ParentId == ParentId);
            predicate = predicate.AndAlsoIf(GuidParentId.HasValue, m => m.GuidParentId == GuidParentId.Value);
            if (Context.MixDbDataAssociation.Any(predicate))
            {
                IsValid = false;
                Errors.Add(new("This association is existed"));
            }
        }
        #endregion

        #region Expands

        #endregion
    }
}
