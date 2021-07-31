using Mix.Database.Entities.Cms.v2;
using Mix.Heart.Enums;
using Mix.Heart.Repository;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Attributes;
using Mix.Portal.Domain.Base;
using System;

namespace Mix.Portal.Domain.ViewModels
{
    [GenerateRestApiController]
    public class MixConfigurationContentViewModel 
        : MultilanguageUniqueNameContentViewModelBase<MixCmsContext, MixConfigurationContent, int>
    {
        public string DefaultContent { get; set; }

        public MixConfigurationContentViewModel()
        {

        }

        public MixConfigurationContentViewModel(Repository<MixCmsContext, MixConfigurationContent, int> repository) : base(repository)
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
