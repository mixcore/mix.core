using Mix.Database.Entities.Cms;
using Mix.Heart.Services;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Base;

namespace Mixcore.Domain.ViewModels
{
    public class PageContentViewModel
        : ExtraColumnMultilanguageSEOContentViewModelBase
            <MixCmsContext, MixPageContent, int, PageContentViewModel>
    {
        #region Contructors

        public PageContentViewModel()
        {
        }

        public PageContentViewModel(MixPageContent entity,
            MixCacheService cacheService = null,
            UnitOfWorkInfo uowInfo = null) : base(entity, cacheService, uowInfo)
        {
        }

        public PageContentViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }
        #endregion

        #region Properties

        public string ClassName { get; set; }

        #endregion
    }
}
