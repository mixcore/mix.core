using Mix.Database.Entities.Cms;
using Mix.Heart.Repository;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Attributes;
using Mix.Lib.Base;
using Mix.Shared.Enums;
using System.Linq;
using System.Threading.Tasks;

namespace Mix.Portal.Domain.ViewModels
{
    [GenerateRestApiController]
    public class MixModuleContentViewModel 
        : ExtraColumnMultilanguageSEOContentViewModelBase<MixCmsContext, MixModuleContent, int>
    {
        #region Contructors

        public MixModuleContentViewModel()
        {
        }

        public MixModuleContentViewModel(Repository<MixCmsContext, MixModuleContent, int> repository) : base(repository)
        {
        }

        public MixModuleContentViewModel(MixModuleContent entity, UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
        {
        }

        public MixModuleContentViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }
        #endregion

        #region Properties

        public string SystemName { get; set; }
        public string ClassName { get; set; }
        public int? PageSize { get; set; }
        public MixModuleType Type { get; set; }

        #endregion

        public override async Task<int> CreateParentAsync()
        {
            MixModuleViewModel parent = new(UowInfo)
            {
                DisplayName = Title,
                SystemName = SystemName,
                Description = Excerpt
            };
            return await parent.SaveAsync();
        }

        protected override async Task DeleteHandlerAsync()
        {
            if (Repository.GetListQuery(m => m.ParentId == ParentId).Count() == 1)
            {
                MixModuleViewModel.Repository = new(UowInfo);

                await Repository.DeleteAsync(Id);
                await MixModuleViewModel.Repository.DeleteAsync(m => m.Id == ParentId);
            }
            else
            {
                await base.DeleteHandlerAsync();
            }
        }
    }
}
