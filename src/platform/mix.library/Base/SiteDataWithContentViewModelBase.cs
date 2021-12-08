using Microsoft.EntityFrameworkCore;
using Mix.Database.Entities.Base;
using Mix.Heart.Entities;
using Mix.Heart.Repository;
using Mix.Heart.Services;
using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mix.Lib.Base
{
    public abstract class SiteDataWithContentViewModelBase
        <TDbContext, TEntity, TPrimaryKey, TView, TContentEntity, TContent>
        : ViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
        where TDbContext : DbContext
        where TEntity : class, IEntity<TPrimaryKey>
        where TPrimaryKey : IComparable
        where TView : ViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
        where TContentEntity : MultilanguageContentBase<TPrimaryKey>
        where TContent : MultilanguageContentViewModelBase<TDbContext, TContentEntity, TPrimaryKey, TContent>
    {
        #region Contructors
        protected SiteDataWithContentViewModelBase()
        {
        }

        protected SiteDataWithContentViewModelBase(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        protected SiteDataWithContentViewModelBase(
            TEntity entity,
            MixCacheService cacheService = null,
            UnitOfWorkInfo uowInfo = null)
            : base(entity, cacheService, uowInfo)
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

        public override async Task ExpandView(MixCacheService cacheService = null)
        {
            using var _contentRepository = 
                ViewModelBase<TDbContext, TContentEntity, TPrimaryKey, TContent>.GetRepository(UowInfo);

            Contents = await _contentRepository.GetListAsync(m => m.ParentId.Equals(Id), cacheService);
        }

        public override void InitDefaultValues(string language = null, int? cultureId = null)
        {
            base.InitDefaultValues(language, cultureId);
            MixTenantId = 1;
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
