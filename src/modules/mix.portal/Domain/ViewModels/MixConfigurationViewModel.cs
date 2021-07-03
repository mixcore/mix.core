using Mix.Database.Entities.Cms.v2;
using Mix.Heart.ViewModel;
using Mix.Lib.Attributes;
using System.Collections.Generic;

namespace Mix.Portal.Domain.ViewModels
{
    [GeneratedController("api/v2/portal/rest/mix-configuration", "Mix Configurations")]
    public class MixConfigurationViewModel : ViewModelBase<MixCmsContext, MixConfiguration, int>
    {
        public virtual string Image { get; set; }
        public virtual string DisplayName { get; set; }
        public virtual string SystemName { get; set; }
        public virtual string Description { get; set; }
        public int MixSiteId { get; set; }

        public IEnumerable<MixConfigurationContentViewModel> Content { get; set; }
    }
}
