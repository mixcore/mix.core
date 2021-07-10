using Mix.Database.Entities.Base;
using Mix.Database.Entities.Cms.v2;
using Mix.Heart.Entities;
using Mix.Heart.Repository;
using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mix.Portal.Domain.Base
{
    public abstract class SiteDataViewModelBase<TEntity, TPrimaryKey, TContentEntity, TContent> : ViewModelBase<MixCmsContext, TEntity, TPrimaryKey>
         where TPrimaryKey : IComparable
        where TEntity : class, IEntity<TPrimaryKey>
        where TContentEntity : MultilanguageSEOContentBase<TPrimaryKey>
        where TContent: SiteContentViewModelBase<TContentEntity, TPrimaryKey>
    {
        #region Contructors

        protected SiteDataViewModelBase(Repository<MixCmsContext, TEntity, TPrimaryKey> repository) : base(repository)
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

        public IEnumerable<TContent> Contents { get; set; }
        #endregion

        #region Overrides

        public override async Task ExtendView()
        {
            var _contentQueryRepository = new QueryRepository<MixCmsContext, TContentEntity, TPrimaryKey>(UowInfo);

            Contents = await _contentQueryRepository.GetListViewAsync<TContent>(
                        m => m.ParentId.Equals(Id), UowInfo);
        }

        #endregion

    }
}
