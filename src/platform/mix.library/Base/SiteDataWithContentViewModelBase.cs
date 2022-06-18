using Microsoft.EntityFrameworkCore;
using Mix.Database.Entities.Base;

namespace Mix.Lib.Base
{
    public abstract class SiteDataWithContentViewModelBase
        <TDbContext, TEntity, TPrimaryKey, TView, TContentEntity, TContent>
        : ViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
        where TDbContext : DbContext
        where TEntity : class, IEntity<TPrimaryKey>
        where TPrimaryKey : IComparable
        where TView : ViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
        where TContentEntity : MultilingualContentBase<TPrimaryKey>
        where TContent : MultilingualContentViewModelBase<TDbContext, TContentEntity, TPrimaryKey, TContent>
    {
        #region Constructors
        protected SiteDataWithContentViewModelBase()
        {
        }

        protected SiteDataWithContentViewModelBase(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        protected SiteDataWithContentViewModelBase(
            TEntity entity,
            UnitOfWorkInfo uowInfo = null)
            : base(entity, uowInfo)
        {
        }

        #endregion

        #region Properties

        public virtual string DisplayName { get; set; }
        public virtual string Description { get; set; }

        public virtual string Image { get; set; }

        public int MixTenantId { get; set; }

        public List<TContent> Contents { get; set; }

        #endregion

        #region Overrides

        public override async Task ExpandView()
        {
            using var _contentRepository =
                ViewModelBase<TDbContext, TContentEntity, TPrimaryKey, TContent>.GetRepository(UowInfo);

            Contents = await _contentRepository.GetListAsync(m => m.ParentId.Equals(Id));
        }

        public override void InitDefaultValues(string language = null, int? cultureId = null)
        {
            base.InitDefaultValues(language, cultureId);
        }

        protected override async Task SaveEntityRelationshipAsync(TEntity parentEntity)
        {
            if (Contents != null)
            {
                foreach (var item in Contents)
                {
                    item.SetUowInfo(UowInfo);
                    item.ParentId = parentEntity.Id;
                    await item.SaveAsync();
                }
            }
        }

        protected override async Task DeleteHandlerAsync()
        {
            using var _contentRepository =
               ViewModelBase<TDbContext, TContentEntity, TPrimaryKey, TContent>.GetRepository(UowInfo);

            await _contentRepository.DeleteManyAsync(m => m.ParentId.Equals(Id));
            await base.DeleteHandlerAsync();
        }
        #endregion
    }
}
