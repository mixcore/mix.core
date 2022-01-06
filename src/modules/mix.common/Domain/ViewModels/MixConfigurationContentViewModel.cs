using Mix.Database.Entities.Cms;
using Mix.Heart.Repository;
using Mix.Heart.Services;
using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;
using Mix.Lib.Attributes;

namespace Mix.Common.Domain.ViewModels
{
    [GenerateRestApiController(QueryOnly = true)]
    public class MixConfigurationContentViewModel
        : ViewModelBase<MixCmsContext, MixConfigurationContent, int, MixConfigurationContentViewModel>
    {
        #region Properties

        public string Specificulture { get; set; }
        public string DisplayName { get; set; }
        public string SystemName { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public int MixCultureId { get; set; }
        public string DefaultContent { get; set; }
        public int MixConfigurationId { get; set; }
        public int MixTenantId { get; set; }

        #endregion

        #region Contructors
        public MixConfigurationContentViewModel()
        {
        }

        public MixConfigurationContentViewModel(MixConfigurationContent entity,
            MixCacheService cacheService = null,
            UnitOfWorkInfo uowInfo = null)
            : base(entity, uowInfo)
        {
        }

        public MixConfigurationContentViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }
        #endregion
    }
}
