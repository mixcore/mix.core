using Mix.Database.Entities.Cms.v2;
using Mix.Heart.Repository;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Attributes;
using Mix.Portal.Domain.Base;
using Mix.Shared.Enums;
using System.Threading.Tasks;

namespace Mix.Portal.Domain.ViewModels
{
    [GenerateRestApiController]
    public class MixModuleContentViewModel 
        : SiteContentSEOViewModelBase<MixCmsContext, MixModuleContent, int>
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

        public string ClassName { get; set; }
        public int? PageSize { get; set; }
        public MixModuleType Type { get; set; }

        #endregion

        public override async Task<int> CreateParentAsync()
        {
            MixModuleViewModel parent = new(UowInfo)
            {
                Title = Title,
                Description = Description
            };
            return await parent.SaveAsync();
        }

    }
}
