using Microsoft.EntityFrameworkCore.Storage;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Heart.Models;
using Mix.Shared.Constants;
using Mix.Lib.Entities.Cms;
using Mix.Shared.Enums;
using Mix.Lib.Services;
using System;
using Mix.Shared.Services;

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
        /// <summary>
        /// Gets the template by path.
        /// </summary>
        /// <param name="path">The path.</param> Ex: "Pages/_Home"
        /// <returns></returns>
        public static RepositoryResponse<MixTemplateViewModel> GetTemplateByPath(string path, string culture
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            RepositoryResponse<MixTemplateViewModel> result = new();
            string[] temp = path.Split('/');
            if (temp.Length < 2)
            {
                result.IsSucceed = false;
                result.Errors.Add("Template Not Found");
            }
            else
            {
                int activeThemeId = ConfigurationService.GetConfig<int>(
                    MixAppSettingKeywords.ThemeId, culture);
                string name = temp[1].Split('.')[0];
                result = Repository.GetSingleModel(t => t.FolderType == temp[0] && t.FileName == name && t.ThemeId == activeThemeId
                    , _context, _transaction);
            }
            return result;
        }

        public static MixTemplateViewModel GetTemplateByPath(int themeId, string path, string type, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            string templateName = path?.Split('/')[1];
            var getView = MixTemplateViewModel.Repository.GetSingleModel(t =>
                    t.ThemeId == themeId && t.FolderType == type
                    && !string.IsNullOrEmpty(templateName) && templateName.Equals($"{t.FileName}{t.Extension}"), _context, _transaction);
            return getView.Data;
        }

        public static MixTemplateViewModel GetDefault(string activedTemplate, string folderType, string folder, string specificulture)
        {
            return new MixTemplateViewModel(new MixTemplate()
            {
                Extension = MixAppSettingService.GetConfig<string>("TemplateExtension"),
                ThemeId = ConfigurationService.GetConfig<int>(MixAppSettingKeywords.ThemeId, specificulture),
                ThemeName = activedTemplate,
                FolderType = folderType,
                FileFolder = folder,
                FileName = MixAppSettingService.GetConfig<string>("DefaultTemplate"),
                Content = "<div></div>"
            });
        }

        #endregion Expand
    }
}
