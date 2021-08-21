using Mix.Database.Entities.Cms;
using Mix.Heart.Repository;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Base;
using System;

namespace Mix.Lib.ViewModels
{
    public class AdditionalDataViewModel 
        : SiteDataWithContentViewModelBase<MixCmsContext, MixData, Guid, MixDataContent, MixDataContentViewModel>
    {
        public AdditionalDataViewModel()
        {
        }

        public AdditionalDataViewModel(Repository<MixCmsContext, MixData, Guid> repository) : base(repository)
        {
        }

        public AdditionalDataViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public AdditionalDataViewModel(MixData entity, UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
        {
        }
    }
}
