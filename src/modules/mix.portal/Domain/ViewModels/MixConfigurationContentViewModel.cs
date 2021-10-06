using Mix.Database.Entities.Cms;
using Mix.Heart.Repository;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Attributes;
using Mix.Lib.Base;

namespace Mix.Portal.Domain.ViewModels
{
    [GenerateRestApiController]
    public class MixConfigurationContentViewModel 
        : MultilanguageUniqueNameContentViewModelBase<MixCmsContext, MixConfigurationContent, int, MixConfigurationContentViewModel>
    {
        public string DefaultContent { get; set; }

        public MixConfigurationContentViewModel()
        {

        }

        public MixConfigurationContentViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public MixConfigurationContentViewModel(MixConfigurationContent entity, UnitOfWorkInfo uowInfo = null) 
            : base(entity, uowInfo)
        {
        }
    }
}
