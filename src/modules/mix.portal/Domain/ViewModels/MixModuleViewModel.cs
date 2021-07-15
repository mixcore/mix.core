using Mix.Database.Entities.Cms.v2;
using Mix.Heart.Repository;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Attributes;
using Mix.Portal.Domain.Base;

namespace Mix.Portal.Domain.ViewModels
{
    [GenerateRestApiController]
    public class MixModuleViewModel : SiteDataWithContentViewModelBase<MixCmsContext, MixModule, int, MixModuleContent, MixModuleContentViewModel>
    {
        #region Contructors

        public MixModuleViewModel()
        {
        }

        public MixModuleViewModel(MixModule entity, UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
        {
        }

        public MixModuleViewModel(Repository<MixCmsContext, MixModule, int> repository) : base(repository)
        {
        }

        public MixModuleViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        #endregion

        #region Overrides


        #endregion
    }
}
