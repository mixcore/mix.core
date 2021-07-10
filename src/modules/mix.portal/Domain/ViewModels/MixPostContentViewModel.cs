using Mix.Database.Entities.Cms.v2;
using Mix.Heart.Repository;
using Mix.Heart.UnitOfWork;
using Mix.Portal.Domain.Base;

namespace Mix.Portal.Domain.ViewModels
{
    public class MixPostContentViewModel : SiteContentViewModelBase<MixPostContent, int>
    {
        #region Contructors
        public MixPostContentViewModel(Repository<MixCmsContext, MixPost, int> repository) : base(repository)
        {
        }

        public MixPostContentViewModel(MixPost entity) : base(entity)
        {
        }

        public MixPostContentViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }
        #endregion

        #region Properties

        public int MixPostId { get; set; }

        public string Specificulture { get; set; }
        public string DisplayName { get; set; }
        public string SystemName { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public int MixCultureId { get; set; }
        public string DefaultContent { get; set; }
        public int MixConfigurationId { get; set; }
        public int MixSiteId { get; set; }

        #endregion
    }
}
