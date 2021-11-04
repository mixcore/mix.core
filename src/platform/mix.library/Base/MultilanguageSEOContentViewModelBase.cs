using Microsoft.EntityFrameworkCore;
using Mix.Heart.Entities;
using Mix.Heart.Enums;
using Mix.Heart.Exceptions;
using Mix.Heart.Repository;
using Mix.Heart.Services;
using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;
using System;
using System.Threading.Tasks;

namespace Mix.Lib.Base
{
    public abstract class MultilanguageSEOContentViewModelBase<TDbContext, TEntity, TPrimaryKey, TView> 
        : MultilanguageContentViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
         where TDbContext : DbContext
         where TPrimaryKey : IComparable
        where TEntity : class, IEntity<TPrimaryKey>
        where TView : ViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
    {
        #region Contructors

        public MultilanguageSEOContentViewModelBase()
        {

        }

        protected MultilanguageSEOContentViewModelBase(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        protected MultilanguageSEOContentViewModelBase(TEntity entity,
            MixCacheService cacheService = null,
            UnitOfWorkInfo uowInfo = null) : base(entity, cacheService, uowInfo)
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

        public virtual Task<TPrimaryKey> CreateParentAsync()
        {
            throw new MixException($"Not implemented CreateParentAsync: {typeof(TView).FullName}");
        }
    }
}
