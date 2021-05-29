using Microsoft.EntityFrameworkCore.Storage;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Lib.Entities.Cms;
using Mix.Lib.Enums;
using System;

namespace Mix.Lib.ViewModels.Cms
{
    public class MixTemplateViewModel : ViewModelBase<MixCmsContext, MixTemplate, MixTemplateViewModel>
    {
        #region Properties
        public int Id { get; set; }
        public string Content { get; set; }
        public string Extension { get; set; }
        public string FileFolder { get; set; }
        public string FileName { get; set; }
        public string FolderType { get; set; }
        public string MobileContent { get; set; }
        public string Scripts { get; set; }
        public string SpaContent { get; set; }
        public string Styles { get; set; }
        public int ThemeId { get; set; }
        public string ThemeName { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public int Priority { get; set; }
        public MixContentStatus Status { get; set; }

        public DateTime CreatedDateTime { get; set; }
        public DateTime? LastModified { get; set; }

        #endregion Properties

        #region Contructors

        public MixTemplateViewModel() : base()
        {
        }

        public MixTemplateViewModel(MixTemplate model, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        #endregion Overrides

        #region Expand

        #endregion Expand
    }
}
