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
        : SiteDataWithContentViewModelBase<MixCmsContext, MixConfiguration, int, MixConfigurationContent, MixConfigurationContentViewModel>
    {
        private QueryRepository<MixCmsContext, MixConfigurationContent, int> _contentQueryRepository;

        #region Contructores

        public MixConfigurationViewModel()
        {

        }

        public MixConfigurationViewModel(Repository<MixCmsContext, MixConfiguration, int> repository) : base(repository)
        {
        }

        public MixConfigurationViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public MixConfigurationViewModel(MixConfiguration entity, UnitOfWorkInfo unitOfWorkInfo = null)
            : base(entity, unitOfWorkInfo)
        {
        }

        #endregion

        #region Properties

        public string SystemName { get; set; }

        #endregion

        #region Overrides

        public override async Task ExpandView()
        {
            _contentQueryRepository = new QueryRepository<MixCmsContext, MixConfigurationContent, int>(UowInfo);

            Contents = await _contentQueryRepository.GetListViewAsync<MixConfigurationContentViewModel>(
                        m => m.ParentId == Id);
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
