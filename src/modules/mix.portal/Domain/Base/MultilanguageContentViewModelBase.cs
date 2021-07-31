using Microsoft.EntityFrameworkCore;
using Mix.Heart.Entities;
using Mix.Heart.Repository;
using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;
using System;

namespace Mix.Portal.Domain.Base
{
    public abstract class MultilanguageContentViewModelBase<TDbContext, TEntity, TPrimaryKey> : ViewModelBase<TDbContext, TEntity, TPrimaryKey>
         where TDbContext : DbContext
         where TPrimaryKey : IComparable
        where TEntity : class, IEntity<TPrimaryKey>
    {
        #region Contructors

        public MultilanguageContentViewModelBase()
        {

        }

        protected MultilanguageContentViewModelBase(Repository<TDbContext, TEntity, TPrimaryKey> repository) : base(repository)
        {
        }

        protected MultilanguageContentViewModelBase(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        protected MultilanguageContentViewModelBase(TEntity entity, UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
        {
        }

        #endregion

        #region Properties

        public string Specificulture { get; set; }

        public TPrimaryKey ParentId { get; set; }
        public int MixCultureId { get; set; }

        #endregion

        #region Overrides

        public override void InitDefaultValues(string language = null, int? cultureId = null)
        {
            base.InitDefaultValues(language, cultureId);
            Specificulture = language;
            MixCultureId = cultureId ?? 1;
        }

        #endregion

    }
}
