using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Mix.Lib.Base
{
    public abstract class SiteDataViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
        : ViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
        where TDbContext : DbContext
        where TPrimaryKey : IComparable
        where TEntity : class, IEntity<TPrimaryKey>
        where TView : ViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
    {
        #region Contructors
        protected SiteDataViewModelBase()
        {
        }

        protected SiteDataViewModelBase(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        protected SiteDataViewModelBase(TEntity entity, UnitOfWorkInfo uowInfo = null)
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
            MixTenantId = MixTenantRepository.Instance.CurrentTenant.Id;
        }

        #endregion



    }
}
