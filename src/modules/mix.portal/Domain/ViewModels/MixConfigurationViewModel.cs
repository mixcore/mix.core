using Mix.Database.Entities.Cms.v2;
using Mix.Heart.Enums;
using Mix.Heart.Repository;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Attributes;
using Mix.Portal.Domain.Base;
using System;
using System.Threading.Tasks;

namespace Mix.Portal.Domain.ViewModels
{
    [GenerateRestApiController(Route = "api/v2/rest/portal/mix-configuration", Name = "Mix Configuration")]
    public class MixConfigurationViewModel
        : SiteDataViewModelBase<MixCmsContext, MixConfiguration, int, MixConfigurationContent, MixConfigurationContentViewModel>
    {
        private QueryRepository<MixCmsContext, MixConfigurationContent, int> _contentQueryRepository;

        public MixConfigurationViewModel()
        {

        }

        public MixConfigurationViewModel(Repository<MixCmsContext, MixConfiguration, int> repository) : base(repository)
        {
        }

        public MixConfigurationViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public MixConfigurationViewModel(MixConfiguration entity) : base(entity)
        {
        }

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
            _contentQueryRepository = new QueryRepository<MixCmsContext, MixConfigurationContent, int>(UowInfo);

            Contents = await _contentQueryRepository.GetListViewAsync<MixConfigurationContentViewModel>(
                        m => m.ParentId == Id, UowInfo);
        }

        protected override async Task SaveEntityRelationshipAsync(MixConfiguration parentEntity)
        {
            if (Contents != null)
            {
                foreach (var item in Contents)
                {
                    item.ParentId = parentEntity.Id;
                    await item.SaveAsync(UowInfo);
                }
            }
        }

        #endregion
    }
}
