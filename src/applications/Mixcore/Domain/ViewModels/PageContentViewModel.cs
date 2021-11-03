using Mix.Database.Entities.Cms;
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

        public PageContentViewModel(MixPageContent entity, UnitOfWorkInfo uowInfo = null)
            : base(entity, uowInfo)
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
