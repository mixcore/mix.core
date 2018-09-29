using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Data.ViewModels;
using System;


namespace Mix.Cms.Lib.ViewModels.MixModules
{
    public class UpdateViewModel:ViewModelBase<MixCmsContext, MixModule, UpdateViewModel>
        
    {
        #region Model

        public int Id { get; set; }
        public string Description { get; set; }
        public string Fields { get; set; }
        public string Image { get; set; }
        public DateTime? LastModified { get; set; }
        public string ModifiedBy { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public string Template { get; set; }
        public string FormTemplate { get; set; }
        public string Title { get; set; }
        public int Type { get; set; }
        public int? PageSize { get; set; }

        #endregion
        #region View

        #endregion
    }
}
