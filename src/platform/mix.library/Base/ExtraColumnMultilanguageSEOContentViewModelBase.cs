using Microsoft.EntityFrameworkCore;
using Mix.Heart.Entities;
using Mix.Heart.Repository;
using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;
using System;

namespace Mix.Lib.Base
{
    public abstract class ExtraColumnMultilanguageSEOContentViewModelBase
        <TDbContext, TEntity, TPrimaryKey, TView> 
        : MultilanguageSEOContentViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
         where TDbContext : DbContext
         where TPrimaryKey : IComparable
        where TEntity : class, IEntity<TPrimaryKey>
        where TView : ViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
    {
        #region Contructors

        public ExtraColumnMultilanguageSEOContentViewModelBase()
        {

        }

        protected ExtraColumnMultilanguageSEOContentViewModelBase(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        protected ExtraColumnMultilanguageSEOContentViewModelBase(TEntity entity, UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
        {
        }

        #endregion

        #region Properties

        public string MixDatabaseName { get; set; }
        public Guid? MixDataContentId { get; set; }

        #endregion

        #region Overrides

        public override void InitDefaultValues(string language = null, int? cultureId = null)
        {
            base.InitDefaultValues(language, cultureId);
        }

        #endregion

    }
}
