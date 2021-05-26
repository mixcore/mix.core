using Microsoft.EntityFrameworkCore.Storage;
using Mix.Lib.Enums;
using Mix.Lib.Entities.Cms;
using Mix.Heart.Infrastructure.ViewModels;
using System;
using Mix.Lib.Attributes;

namespace Mix.Theme.Domain.ViewModels
{
    [GeneratedController("api/v2/rest/{culture}/mix-theme", "Mix Theme Portal")]
    public class MixThemeViewModel : ViewModelBase<MixCmsContext, MixTheme, MixThemeViewModel>
    {
        #region Properties

        #region Models

        public int Id { get; set; }

        public string Specificulture { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string Image { get; set; }

        public string Thumbnail { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? LastModified { get; set; }

        public int Priority { get; set; }

        public MixContentStatus Status { get; set; }

        #endregion Models

        #region Views

        //public bool IsCloneFromCurrentTheme { get; set; }

        //public bool IsActived { get; set; }

        //public FileViewModel TemplateAsset { get; set; }

        //public FileViewModel Asset { get; set; }

        //public string AssetFolder
        //{
        //    get
        //    {
        //        return $"{MixFolders.SiteContentAssetsFolder}/{Name}/assets";
        //    }
        //}

        //public string UploadsFolder
        //{
        //    get
        //    {
        //        return $"{MixFolders.SiteContentAssetsFolder}/{Name}/uploads";
        //    }
        //}

        //public string TemplateFolder
        //{
        //    get
        //    {
        //        return $"{MixFolders.TemplatesFolder}/{Name}";
        //    }
        //}

        //public List<MixTemplates.UpdateViewModel> Templates { get; set; }

        #endregion Views

        #endregion Properties

        #region Contructors

        public MixThemeViewModel()
            : base()
        {
        }

        public MixThemeViewModel(MixTheme model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
        }

        #endregion Contructors
    }
}
