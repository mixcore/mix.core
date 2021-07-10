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
    public class MixPostViewModel : SiteDataViewModelBase<MixCmsContext, MixPost, int, MixPostContent, MixPostContentViewModel>
    {

        #region Contructors

        public MixPostViewModel()
        {
        }

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

       
        #endregion
    }
}
