using Microsoft.EntityFrameworkCore;
using Mix.Constant.Constants;
using Mix.Heart.Entities;
using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;

namespace Mix.Mixdb.ViewModels
{
    public abstract class MixDbViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
        : ViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
        where TPrimaryKey : IComparable
        where TEntity : class, IEntity<TPrimaryKey>
        where TDbContext : DbContext
        where TView : ViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
    {
        protected MixDbViewModelBase(string mixdbName) : base()
        {
            CacheFolder = $"{MixFolders.MixDbCacheFolder}/{mixdbName}";
        }

        protected MixDbViewModelBase(string mixdbName, TDbContext context) : base(context)
        {
            CacheFolder = $"{MixFolders.MixDbCacheFolder}/{mixdbName}";
        }

        protected MixDbViewModelBase(string mixdbName, UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
            CacheFolder = $"{MixFolders.MixDbCacheFolder}/{mixdbName}";
        }

        protected MixDbViewModelBase(string mixdbName, TEntity entity, UnitOfWorkInfo uowInfo) : base(entity, uowInfo)
        {
            CacheFolder = $"{MixFolders.MixDbCacheFolder}/{mixdbName}";
        }


    }
}
