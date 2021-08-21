using Microsoft.EntityFrameworkCore;
using Mix.Heart.Entities;
using Mix.Heart.Repository;
using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;
using System;

namespace Mix.Lib.Base
{
    public abstract class MultilanguageUniqueNameContentViewModelBase<TDbContext, TEntity, TPrimaryKey> 
        : MultilanguageContentViewModelBase<TDbContext, TEntity, TPrimaryKey>
         where TDbContext : DbContext
         where TPrimaryKey : IComparable
        where TEntity : class, IEntity<TPrimaryKey>
    {
        #region Contructors

        public MultilanguageUniqueNameContentViewModelBase()
        {

        }

        protected MultilanguageUniqueNameContentViewModelBase(Repository<TDbContext, TEntity, TPrimaryKey> repository) : base(repository)
        {
        }

        protected MultilanguageUniqueNameContentViewModelBase(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        protected MultilanguageUniqueNameContentViewModelBase(TEntity entity, UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
        {
        }

        #endregion

        #region Properties

        public string DisplayName { get; set; }
        public string SystemName { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }

        #endregion

        #region Overrides

        public override void InitDefaultValues(string language = null, int? cultureId = null)
        {
            base.InitDefaultValues(language, cultureId);
        }

        #endregion

    }
}
