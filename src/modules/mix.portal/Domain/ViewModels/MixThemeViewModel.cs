using Mix.Database.Entities.Cms;
using Mix.Heart.Repository;
using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mix.Portal.Domain.ViewModels
{
    public class MixThemeViewModel 
        : ViewModelBase<MixCmsContext, MixTheme, int>
    {
        #region Properties

        public string PreviewUrl { get; set; }
        public string MixDatabaseName { get; set; }
        public Guid MixDataContentId { get; set; }

        public List<MixViewTemplateViewModel> Templates { get; set; }
        #endregion

        #region Contructors

        public MixThemeViewModel()
        {
        }

        public MixThemeViewModel(MixTheme entity, UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
        {
        }

        public MixThemeViewModel(Repository<MixCmsContext, MixTheme, int> repository) : base(repository)
        {
        }

        public MixThemeViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        #endregion

        #region Overrides

        #endregion

        #region Expands

        public async Task LoadTemplates(UnitOfWorkInfo uowInfo = null)
        {
            UowInfo ??= uowInfo;
            var templateRepo = new QueryRepository<MixCmsContext, MixViewTemplate, int>(UowInfo);
            Templates = await templateRepo.GetListViewAsync<MixViewTemplateViewModel>(
                            m => m.MixThemeId == Id);
        }

        #endregion
    }
}
