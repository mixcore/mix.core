using Mix.Database.Entities.Cms;
using Mix.Heart.Repository;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Attributes;
using Mix.Lib.Base;
using System.Linq;
using System.Threading.Tasks;

namespace Mix.Portal.Domain.ViewModels
{
    [GenerateRestApiController]
    public class MixPostContentViewModel
        : ExtraColumnMultilanguageSEOContentViewModelBase<MixCmsContext, MixPostContent, int, MixPostContentViewModel>
    {
        #region Contructors

        public MixPostContentViewModel()
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

        public override async Task<int> CreateParentAsync()
        {
            MixPostViewModel parent = new(UowInfo)
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
                var postRepo = MixPostViewModel.GetRepository(UowInfo);
                await Repository.DeleteAsync(Id);
                await postRepo.DeleteAsync(m => m.Id == ParentId);
            }
            else
            {
                await base.DeleteHandlerAsync();
            }
        }
    }
}
