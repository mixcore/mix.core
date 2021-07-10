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
    [GenerateRestApiController(Route = "api/v2/rest/portal/mix-Post", Name = "Mix Post")]
    public class MixPostViewModel : SiteDataViewModelBase<MixPost, int, MixPostContent, MixPostContentViewModel>
    {
        private QueryRepository<MixCmsContext, MixPostContent, int> _contentQueryRepository;

        #region Contructors

        public MixPostViewModel(MixPost entity) : base(entity)
        {   
        }

        public MixPostViewModel(Repository<MixCmsContext, MixPost, int> repository) : base(repository)
        {
        }

        public MixPostViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        #endregion

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

        

        protected override async Task SaveEntityRelationshipAsync(MixPost parentEntity)
        {
            if (Contents!=null)
            {
                foreach (var item in Contents)
                {
                    item.MixPostId = parentEntity.Id;
                    await item.SaveAsync(UowInfo);
                }
            }
        }

        #endregion
    }
}
