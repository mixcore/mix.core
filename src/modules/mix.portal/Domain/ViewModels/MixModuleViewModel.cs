using System.ComponentModel.DataAnnotations;

namespace Mix.Portal.Domain.ViewModels
{
    [GenerateRestApiController]
    public class MixModuleViewModel 
        : SiteDataWithContentViewModelBase<MixCmsContext, MixModule, int, MixModuleViewModel, MixModuleContent, MixModuleContentViewModel>
    {
        #region Contructors

        public MixModuleViewModel()
        {
        }

        public MixModuleViewModel(MixModule entity,
            MixCacheService cacheService = null,
            UnitOfWorkInfo uowInfo = null) : base(entity, cacheService, uowInfo)
        {
        }

        public MixModuleViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        #endregion

        #region Properties

        [Required]
        public string SystemName { get; set; }

        #endregion

        #region Overrides


        #endregion
    }
}
