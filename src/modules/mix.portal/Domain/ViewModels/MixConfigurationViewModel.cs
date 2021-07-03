using Mix.Database.Entities.Cms.v2;
using Mix.Heart.Enums;
using Mix.Heart.ViewModel;
using Mix.Lib.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public MixConfigurationViewModel() : base()
        {

        }

        public MixConfigurationViewModel(MixConfiguration entity) : base(entity)
        {
        }

        public IEnumerable<MixConfigurationContentViewModel> Content { get; set; }

        #region Overrides

        protected override void InitEntityValues()
        {
            if (Id == default)
            {
                Id = _repository.MaxAsync(m => m.Id);
                MixSiteId = 1;
                CreatedDateTime = DateTime.UtcNow;
                Status = MixContentStatus.Published;
            }
        }

        protected override async Task SaveEntityRelationshipAsync(MixConfiguration parentEntity)
        {
            foreach (var item in Content)
            {
                item.MixConfigurationId = parentEntity.Id;
                await item.SaveAsync(_unitOfWorkInfo);
            }
        }

        #endregion
    }
}
