using Mix.Database.Entities.Cms.v2;
using Mix.Heart.Entities;
using Mix.Heart.Repository;
using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;
using System;

namespace Mix.Portal.Domain.Base
{
    public abstract class SiteContentViewModelBase<TEntity, TPrimaryKey> : ViewModelBase<MixCmsContext, TEntity, TPrimaryKey>
         where TPrimaryKey : IComparable
        where TEntity : class, IEntity<TPrimaryKey>
    {
        #region Contructors

        protected SiteContentViewModelBase(Repository<MixCmsContext, TEntity, TPrimaryKey> repository) : base(repository)
        {
        }

        protected SiteContentViewModelBase(TEntity entity) : base(entity)
        {
        }

        protected SiteContentViewModelBase(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        #endregion

        #region Properties

        public string Layout { get; set; }
        public string Template { get; set; }
        public string Image { get; set; }
        public string Source { get; set; }
        public string SeoDescription { get; set; }
        public string SeoKeywords { get; set; }
        public string SeoName { get; set; }
        public string SeoTitle { get; set; }
        public DateTime? PublishedDateTime { get; set; }
        public string MixDatabaseName { get; set; }

        public TPrimaryKey ParentId { get; set; }
        public Guid MixDataContentId { get; set; }

        #endregion

    }
}
