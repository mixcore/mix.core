using Microsoft.EntityFrameworkCore;

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

        public override async Task ExpandView(CancellationToken cancellationToken = default)
        {
            using (var contentRepository = ViewModelBase<TDbContext, TContentEntity, TPrimaryKey, TContent>.GetRepository(UowInfo, CacheService))
            {
                Contents = await contentRepository.GetListAsync(m => m.ParentId.Equals(Id), cancellationToken);
                contentRepository.Dispose();
            }
        }

        protected override async Task SaveEntityRelationshipAsync(TEntity parentEntity, CancellationToken cancellationToken = default)
        {
            if (Contents != null)
            {
                foreach (var item in Contents)
                {
                    item.SetUowInfo(UowInfo, CacheService);
                    item.ParentId = parentEntity.Id;
                    await item.SaveAsync(cancellationToken);
                    ModifiedEntities.AddRange(item.ModifiedEntities);
                }
            }
        }

        protected override async Task DeleteHandlerAsync(CancellationToken cancellationToken = default)
        {
            using (var contentRepository = ViewModelBase<TDbContext, TContentEntity, TPrimaryKey, TContent>.GetRepository(UowInfo, CacheService))
            {
                await contentRepository.DeleteManyAsync(m => m.ParentId.Equals(Id), cancellationToken);
                await base.DeleteHandlerAsync(cancellationToken);
                contentRepository.Dispose();
            }
        }
        #endregion
    }
}
