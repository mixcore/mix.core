using Mix.Database.Entities.Cms.v2;
using Mix.Heart.Enums;
using Mix.Heart.Repository;
using Mix.Heart.ViewModel;
using Mix.Lib.Attributes;
using System;
using System.Linq;

namespace Mix.Portal.Domain.ViewModels
{
    [GeneratedController]
    public class MixConfigurationContentViewModel : ViewModelBase<MixCmsContext, MixConfigurationContent, int>
    {
        public string Specificulture { get; set; }
        public string DisplayName { get; set; }
        public string SystemName { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public int MixCultureId { get; set; }
        public string DefaultContent { get; set; }
        public int MixConfigurationId { get; set; }
        public int MixSiteId { get; set; }

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
                MixSiteId = 1;
                MixCultureId = 1;
                CreatedDateTime = DateTime.UtcNow;
                Status = MixContentStatus.Published;
            }
        }
    }
}
