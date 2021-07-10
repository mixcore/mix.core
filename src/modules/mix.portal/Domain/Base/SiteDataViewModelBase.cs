using Microsoft.EntityFrameworkCore;
using Mix.Database.Entities.Base;
using Mix.Heart.Entities;
using Mix.Heart.Enums;
using Mix.Heart.Repository;
using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mix.Portal.Domain.Base
{
    public abstract class SiteDataViewModelBase<TDbContext, TEntity, TPrimaryKey, TContentEntity, TContent> : ViewModelBase<TDbContext, TEntity, TPrimaryKey>
        where TDbContext : DbContext
         where TPrimaryKey : IComparable
        where TEntity : class, IEntity<TPrimaryKey>
        where TContentEntity : MultilanguageContentBase<TPrimaryKey>
        where TContent : SiteContentViewModelBase<TDbContext, TContentEntity, TPrimaryKey>
    {
        #region Contructors
        protected SiteDataViewModelBase()
        {
        }

        protected SiteDataViewModelBase(Repository<TDbContext, TEntity, TPrimaryKey> repository) : base(repository)
        {
        }

        protected SiteDataViewModelBase(TEntity entity) : base(entity)
        {
        }

        protected SiteDataViewModelBase(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        #endregion

        #region Properties

        public virtual string Image { get; set; }
        public virtual string DisplayName { get; set; }
        public virtual string SystemName { get; set; }
        public virtual string Description { get; set; }
        public int MixSiteId { get; set; }

        public List<TContent> Contents { get; set; }

        #endregion

        #region Overrides

        public override async Task ExtendView()
        {
            var _contentQueryRepository = new QueryRepository<TDbContext, TContentEntity, TPrimaryKey>(UowInfo);

            Contents = await _contentQueryRepository.GetListViewAsync<TContent>(
                        m => m.ParentId.Equals(Id), UowInfo);
        }

        public override void InitDefaultValues(string language = null, int? cultureId = null)
        {
            MixSiteId = 1;
            CreatedDateTime = DateTime.UtcNow;
            Status = MixContentStatus.Published;

            if (Contents == null)
            {
                Contents = new();
                var content = (TContent)Activator.CreateInstance(typeof(TContent));
                content.InitDefaultValues(language, cultureId);
                Contents.Add(content);
            }
        }

        protected override async Task SaveEntityRelationshipAsync(TEntity parentEntity)
        {
            if (Contents != null)
            {
                foreach (var item in Contents)
                {
                    item.ParentId = parentEntity.Id;
                    await item.SaveAsync(UowInfo);
                }
            }
        }
        #endregion



    }
}
