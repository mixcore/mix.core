using Mix.Database.Entities.Cms;
using Mix.Heart.Repository;
using Mix.Heart.Services;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Attributes;
using Mix.Lib.Base;
using System.Linq;
using System.Threading.Tasks;

namespace Mix.Portal.Domain.ViewModels
{
    [GenerateRestApiController]
    public class MixPageContentViewModel 
        : ExtraColumnMultilanguageSEOContentViewModelBase
            <MixCmsContext, MixPageContent, int, MixPageContentViewModel>
    {
        #region Contructors

        public MixPageContentViewModel()
        {
        }

        public MixPageContentViewModel(MixPageContent entity,
            MixCacheService cacheService = null,
            UnitOfWorkInfo uowInfo = null) : base(entity, cacheService, uowInfo)
        {
        }

        public MixPageContentViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }
        #endregion

        #region Properties

        public string ClassName { get; set; }

        #endregion

        public override async Task<int> CreateParentAsync()
        {
            MixPageViewModel parent = new(UowInfo)
            {
                DisplayName = Title,
                Description = Excerpt
            };
            return await parent.SaveAsync();
        }

        protected override async Task DeleteHandlerAsync()
        {
            if (Repository.GetListQuery(m => m.ParentId == ParentId).Count() == 1)
            {
                var pageRepo = MixPageViewModel.GetRepository(UowInfo);

                await Repository.DeleteAsync(Id);
                await pageRepo.DeleteAsync(m => m.Id == ParentId);
            }
            else
            {
                await base.DeleteHandlerAsync();
            }
        }
    }
}
