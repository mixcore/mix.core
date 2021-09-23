using Mix.Database.Entities.Cms;
using Mix.Heart.Repository;
using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;
using Mix.Lib.Attributes;

namespace Mix.Common.Domain.ViewModels
{
    [GenerateRestApiController(QueryOnly = true)]
    public class MixConfigurationContentViewModel
        : ViewModelBase<MixCmsContext, MixConfigurationContent, int>
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

        public MixConfigurationContentViewModel(Repository<MixCmsContext, MixConfigurationContent, int> repository) : base(repository)
        {
        }

        public MixConfigurationContentViewModel(MixConfigurationContent entity, UnitOfWorkInfo uowInfo) : base(entity, uowInfo)
        {
        }

        public MixConfigurationContentViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }
        #endregion
    }
}
