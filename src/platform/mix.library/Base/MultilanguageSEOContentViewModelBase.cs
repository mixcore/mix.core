using Microsoft.EntityFrameworkCore;
using Mix.Heart.Entities;
using Mix.Heart.Enums;
using Mix.Heart.Repository;
using Mix.Heart.UnitOfWork;
using System;
using System.Threading.Tasks;

namespace Mix.Lib.Base
{
    public abstract class MultilanguageSEOContentViewModelBase<TDbContext, TEntity, TPrimaryKey> 
        : MultilanguageContentViewModelBase<TDbContext, TEntity, TPrimaryKey>
        where TDbContext : DbContext
        where TPrimaryKey : IComparable
        where TEntity : class, IEntity<TPrimaryKey>
    {
        #region Contructors

        public MultilanguageSEOContentViewModelBase()
        {

        }

        protected MultilanguageSEOContentViewModelBase(Repository<TDbContext, TEntity, TPrimaryKey> repository) : base(repository)
        {
        }

        protected MultilanguageSEOContentViewModelBase(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        protected MultilanguageSEOContentViewModelBase(TEntity entity, UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
        {
        }

        #endregion

        #region Properties

        public string Title { get; set; }
        public string Excerpt { get; set; }
        public string Content { get; set; }
        public string Layout { get; set; }
        public string Template { get; set; }
        public string Image { get; set; }
        public string Source { get; set; }
        public string SeoDescription { get; set; }
        public string SeoKeywords { get; set; }
        public string SeoName { get; set; }
        public string SeoTitle { get; set; }
        public DateTime? PublishedDateTime { get; set; }
        
        #region Extra

        public bool IsClone { get; set; }

        #endregion

        #endregion

        #region Overrides

        public override void InitDefaultValues(string language = null, int? cultureId = null)
        {
            Status = MixContentStatus.Published;
            Specificulture = language ?? Specificulture;
            MixCultureId = cultureId ?? MixCultureId;
        }

        protected override async Task<TEntity> SaveHandlerAsync()
        {
            if (IsDefaultId(ParentId))
            {
                ParentId = await CreateParentAsync();
            }
            return await base.SaveHandlerAsync();
        }

        #endregion

        public abstract Task<TPrimaryKey> CreateParentAsync();
    }
}
