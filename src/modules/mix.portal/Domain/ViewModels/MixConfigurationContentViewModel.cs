using Mix.Database.Entities.Cms.v2;
using Mix.Heart.Enums;
using Mix.Heart.Repository;
using Mix.Lib.Attributes;
using Mix.Portal.Domain.Base;
using System;

namespace Mix.Portal.Domain.ViewModels
{
    [GenerateRestApiController]
    public class MixConfigurationContentViewModel : SiteContentViewModelBase<MixCmsContext, MixConfigurationContent, int>
    {
        public string DefaultContent { get; set; }

        public MixConfigurationContentViewModel()
        {

        }

        public MixConfigurationContentViewModel(MixConfigurationContent entity) : base(entity)
        {
        }

        public MixConfigurationContentViewModel(Repository<MixCmsContext, MixConfigurationContent, int> repository) : base(repository)
        {
        }

        protected override void InitEntityValues()
        {
            if (Id == default)
            {
                MixCultureId = 1;
                CreatedDateTime = DateTime.UtcNow;
                Status = MixContentStatus.Published;
            }
        }
    }
}
