using Mix.Database.Entities.Cms;
using Mix.Heart.Repository;
using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;
using Mix.Shared.Enums;
using System;

namespace Mix.Portal.Domain.ViewModels
{
    public class MixViewTemplateViewModel 
        : ViewModelBase<MixCmsContext, MixViewTemplate, int>
    {
        #region Properties

        public string Content { get; set; }
        public string Extension { get; set; }
        public string FileFolder { get; set; }
        public string FileName { get; set; }
        public MixTemplateFolderType FolderType { get; set; }
        public string Scripts { get; set; }
        public string SpaContent { get; set; }
        public string MobileContent { get; set; }
        public string Styles { get; set; }

        #endregion

        #region Contructors

        public MixViewTemplateViewModel()
        {
        }

        public MixViewTemplateViewModel(MixViewTemplate entity, UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
        {
        }

        public MixViewTemplateViewModel(Repository<MixCmsContext, MixViewTemplate, int> repository) : base(repository)
        {
        }

        public MixViewTemplateViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        #endregion

        #region Overrides

        #endregion
    }
}
