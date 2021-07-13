using Mix.Database.Entities.Cms.v2;
using Mix.Heart.Repository;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Attributes;
using Mix.Portal.Domain.Base;

namespace Mix.Portal.Domain.ViewModels
{
    [GenerateRestApiController]
    public class MixPostContentViewModel 
        : SiteContentSEOViewModelBase<MixCmsContext, MixPostContent, int>
    {
        #region Contructors

        public MixPostContentViewModel()
        {
        }

        public MixPostContentViewModel(Repository<MixCmsContext, MixPostContent, int> repository) : base(repository)
        {
        }

        public MixPostContentViewModel(MixPostContent entity, UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
        {
        }

        public MixPostContentViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }
        #endregion

        #region Properties

        public string ClassName { get; set; }

        #endregion

    }
}
