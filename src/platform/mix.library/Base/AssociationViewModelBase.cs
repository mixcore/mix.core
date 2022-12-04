using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Mix.Lib.Base
{
    public abstract class AssociationViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
        : ViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
        where TDbContext : DbContext
        where TPrimaryKey : IComparable
        where TEntity : AssociationBase<TPrimaryKey>
        where TView : ViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
    {
        #region Constructors
        protected AssociationViewModelBase()
        {
        }

        protected AssociationViewModelBase(UnitOfWorkInfo unitOfWorkInfo) 
            : base(unitOfWorkInfo)
        {
        }

        protected AssociationViewModelBase(TEntity entity, UnitOfWorkInfo uowInfo = null)
            : base(entity, uowInfo)
        {
        }

        #endregion

        #region Properties

        public TPrimaryKey ParentId { get; set; }
        public TPrimaryKey ChildId { get; set; }
        public int MixTenantId { get; set; }

        #endregion

        #region Overrides

        public override async Task Validate(CancellationToken cancellationToken)
        {

            if (Repository.Table.Any(m => !m.Id.Equals(Id) && m.MixTenantId == MixTenantId && m.ParentId.Equals(ParentId) && m.ChildId.Equals(ChildId)))
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

            await base.Validate(cancellationToken);

        }
        #endregion
    }
}
