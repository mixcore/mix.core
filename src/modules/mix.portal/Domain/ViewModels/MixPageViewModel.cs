using Mix.Database.Entities.Cms.v2;
using Mix.Heart.Repository;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Attributes;
using Mix.Portal.Domain.Base;

namespace Mix.Portal.Domain.ViewModels
{
    [GenerateRestApiController]
    public class MixPageViewModel : SiteDataWithContentViewModelBase<MixCmsContext, MixPage, int, MixPageContent, MixPageContentViewModel>
    {
        #region Contructors

        public MixPageViewModel()
        {
        }

        public MixPageViewModel(MixPage entity, UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
        {
        }

        public MixPageViewModel(Repository<MixCmsContext, MixPage, int> repository) : base(repository)
        {
        }

        public MixPageViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        #endregion

        #region Overrides


        #endregion
    }
}
