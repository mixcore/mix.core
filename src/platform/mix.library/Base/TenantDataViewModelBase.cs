using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Mix.Lib.Base
{
    public abstract class TenantDataViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
        : ViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
        where TDbContext : DbContext
        where TPrimaryKey : IComparable
        where TEntity : class, IEntity<TPrimaryKey>
        where TView : ViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
    {
        #region Constructors
        protected TenantDataViewModelBase()
        {
        }

        protected TenantDataViewModelBase(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        protected TenantDataViewModelBase(TEntity entity, UnitOfWorkInfo uowInfo = null)
            : base(entity, uowInfo)
        {
        }

        #endregion

        #region Properties

        public string Description { get; set; }

        public virtual string Image { get; set; }
        [Required]
        public virtual string DisplayName { get; set; }
        public int MixTenantId { get; set; }

        #endregion

        #region Overrides

        public override void InitDefaultValues(string language = null, int? cultureId = null)
        {
            base.InitDefaultValues(language, cultureId);
        }

        #endregion



    }
}
