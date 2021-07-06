using Mix.Database.Entities.Cms.v2;
using Mix.Heart.Enums;
using Mix.Heart.Repository;
using Mix.Heart.ViewModel;
using Mix.Lib.Attributes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mix.Heart.Extensions;

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

        private QueryRepository<MixCmsContext, MixConfigurationContent, int> _contentQueryRepository;

        public MixConfigurationViewModel(MixConfiguration entity) : base(entity)
        {   
        }

        public MixConfigurationViewModel(Repository<MixCmsContext, MixConfiguration, int> repository) : base(repository)
        {
        }

        public IEnumerable<MixConfigurationContentViewModel> Content { get; set; }

        #region Overrides

        protected override void InitEntityValues()
        {
            if (Id == default)
            {
                MixSiteId = 1;
                CreatedDateTime = DateTime.UtcNow;
                Status = MixContentStatus.Published;
            }
        }

        public override async Task ExtendView()
        {
            _contentQueryRepository = new QueryRepository<MixCmsContext, MixConfigurationContent, int>(_unitOfWorkInfo);

            Content = await _contentQueryRepository.GetListViewAsync<MixConfigurationContentViewModel>(
                        m => m.MixConfigurationId == Id, _unitOfWorkInfo);
        }

        protected override async Task SaveEntityRelationshipAsync(MixConfiguration parentEntity)
        {
            if (Content!=null)
            {
                foreach (var item in Content)
                {
                    item.MixConfigurationId = parentEntity.Id;
                    await item.SaveAsync(_unitOfWorkInfo);
                }
            }
        }

        #endregion
    }
}
