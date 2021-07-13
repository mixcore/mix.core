using Microsoft.EntityFrameworkCore;
using Mix.Database.Entities.Base;
using Mix.Heart.Entities;
using Mix.Heart.Enums;
using Mix.Heart.Repository;
using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Mix.Portal.Domain.Base
{
    public abstract class SiteDataWithContentViewModelBase<TDbContext, TEntity, TPrimaryKey>
        : ViewModelBase<TDbContext, TEntity, TPrimaryKey>
        where TDbContext : DbContext
         where TPrimaryKey : IComparable
        where TEntity : class, IEntity<TPrimaryKey>
    {
        #region Contructors
        protected SiteDataWithContentViewModelBase()
        {
        }

        protected SiteDataWithContentViewModelBase(Repository<TDbContext, TEntity, TPrimaryKey> repository) : base(repository)
        {
        }

        protected SiteDataWithContentViewModelBase(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        protected SiteDataWithContentViewModelBase(TEntity entity, UnitOfWorkInfo uowInfo) : base(entity, uowInfo)
        {
        }

        #endregion

        #region Properties

        public virtual string Image { get; set; }
        [Required]
        public virtual string DisplayName { get; set; }
        [Required]
        public virtual string SystemName { get; set; }
        public virtual string Description { get; set; }
        public int MixSiteId { get; set; }

        #endregion

        #region Overrides

        public override void InitDefaultValues(string language = null, int? cultureId = null)
        {
            base.InitDefaultValues(language, cultureId);
            MixSiteId = 1;
        }

        #endregion



    }
}
