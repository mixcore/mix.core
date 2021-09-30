using Mix.Database.Entities.Cms;
using Mix.Heart.Repository;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Attributes;
using Mix.Lib.Base;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mix.Portal.Domain.ViewModels
{
    [GenerateRestApiController]
    public class MixPostContentViewModel
        : ExtraColumnMultilanguageSEOContentViewModelBase<MixCmsContext, MixPostContent, int>
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
                MixPostViewModel.Repository = new(UowInfo);

                await Repository.DeleteAsync(Id);
                await MixPostViewModel.Repository.DeleteAsync(m => m.Id == ParentId);
            }
            else
            {
                await base.DeleteHandlerAsync();
            }
        }
    }
}
