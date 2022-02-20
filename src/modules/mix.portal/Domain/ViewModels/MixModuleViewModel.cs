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

            UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
        {
        }

        public MixModuleViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        #endregion

        #region Properties

        [Required]
        public string SystemName { get; set; }

        public virtual MixModuleType Type { get; set; }

        #endregion

        #region Overrides


        #endregion
    }
}
