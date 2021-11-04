using Mix.Database.Entities.Cms;
using Mix.Heart.Repository;
using Mix.Heart.Services;
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
        : ExtraColumnMultilanguageSEOContentViewModelBase<MixCmsContext, MixModuleContent, int, MixModuleContentViewModel>
    {
        #region Contructors

        public MixModuleContentViewModel()
        {
        }

        public MixModuleContentViewModel(MixModuleContent entity,
            MixCacheService cacheService = null,
            UnitOfWorkInfo uowInfo = null) : base(entity, cacheService, uowInfo)
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
                var mdlRepo = MixModuleViewModel.GetRepository(UowInfo);

                await Repository.DeleteAsync(Id);
                await mdlRepo.DeleteAsync(m => m.Id == ParentId);
            }
            else
            {
                await base.DeleteHandlerAsync();
            }
        }
    }
}
