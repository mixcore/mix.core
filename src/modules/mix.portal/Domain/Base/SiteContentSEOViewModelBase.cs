using Microsoft.EntityFrameworkCore;
using Mix.Heart.Entities;
using Mix.Heart.Repository;
using Mix.Heart.UnitOfWork;
using System;

namespace Mix.Portal.Domain.Base
{
    public abstract class SiteContentSEOViewModelBase<TDbContext, TEntity, TPrimaryKey> : SiteContentViewModelBase<TDbContext, TEntity, TPrimaryKey>
        where TDbContext : DbContext
        where TPrimaryKey : IComparable
        where TEntity : class, IEntity<TPrimaryKey>
    {
        #region Contructors

        public SiteContentSEOViewModelBase()
        {

        }

        protected SiteContentSEOViewModelBase(Repository<TDbContext, TEntity, TPrimaryKey> repository) : base(repository)
        {
        }

        protected SiteContentSEOViewModelBase(TEntity entity) : base(entity)
        {
        }

        protected SiteContentSEOViewModelBase(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
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

        public Guid MixDataContentId { get; set; }

        #endregion

    }
}
