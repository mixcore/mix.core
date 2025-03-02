using Microsoft.EntityFrameworkCore;

namespace Mix.Lib.Base
{
    public abstract class MultilingualUniqueNameContentViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
        : MultilingualContentViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
        where TDbContext : DbContext
        where TPrimaryKey : IComparable
        where TEntity : MultilingualContentBase<TPrimaryKey>
        where TView : MultilingualUniqueNameContentViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
    {
        #region Constructors

        protected MultilingualUniqueNameContentViewModelBase()
        {

        }

        protected MultilingualUniqueNameContentViewModelBase(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        protected MultilingualUniqueNameContentViewModelBase(TEntity entity,
            UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
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
        #endregion
    }
}
