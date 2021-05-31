using Microsoft.EntityFrameworkCore.Storage;
using Mix.Common.Helper;
using Mix.Heart.Models;
using Mix.Lib;
using Mix.Lib.Abstracts.ViewModels;
using Mix.Lib.Constants;
using Mix.Lib.Entities.Cms;
using Mix.Lib.Enums;
using Mix.Lib.Helpers;
using Mix.Lib.Models.Common;
using Mix.Lib.Services;
using Mix.Lib.ViewModels.Cms;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Mixcore.Domain.ViewModels.Mvc
{
    public class MvcPostViewModel : MixPostViewModelBase<MvcPostViewModel>
    {
        #region Properties
        public string DetailsUrl { get => Id > 0 ? $"/{Specificulture}/post/{Id}/{SeoName}" : null; }

        public MixTemplateViewModel View { get; set; }

        public string Domain { get { return MixAppSettingService.GetConfig<string>(MixAppSettingKeywords.Domain); } }

        public string ImageUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(Image) && (!Image.Contains("http", StringComparison.CurrentCulture)) && Image[0] != '/')
                {
                    return $"{Domain}/{Image}";
                }
                else
                {
                    return Image;
                }
            }
        }

        public string ThumbnailUrl
        {
            get
            {
                if (Thumbnail != null && !Thumbnail.Contains("http", StringComparison.CurrentCulture) && Thumbnail[0] != '/')
                {
                    return $"{Domain}/{Thumbnail}";
                }
                else
                {
                    return string.IsNullOrEmpty(Thumbnail) ? ImageUrl : Thumbnail;
                }
            }
        }

        public string TemplatePath
        {
            get
            {
                return $"/{ MixFolders.TemplatesFolder}/{MixAppSettingService.GetConfig<string>(MixAppSettingKeywords.ThemeFolder, Specificulture) ?? "Default"}/{Template}";
            }
        }

        //public List<MixPostMedias.ReadViewModel> MediaNavs { get; set; }

        //public List<MixPostModules.ReadViewModel> ModuleNavs { get; set; }

        //public List<MixPostPosts.ReadViewModel> PostNavs { get; set; }

        //public List<MixDatabases.ReadViewModel> Databases { get; set; } = new List<MixDatabases.ReadViewModel>();

        public MixDatabaseDataViewModel AttributeData { get; set; }

        //public List<MixDatabaseDataAssociations.FormViewModel> SysTags { get; set; } = new List<MixDatabaseDataAssociations.FormViewModel>();

        //public List<MixDatabaseDataAssociations.FormViewModel> SysCategories { get; set; } = new List<MixDatabaseDataAssociations.FormViewModel>();

        //public List<string> ListTag { get => SysTags.Select(t => t.AttributeData?.Property<string>("title")).Distinct().ToList(); }

        //public List<string> ListCategory { get => SysCategories.Select(t => t.AttributeData?.Property<string>("title")).Distinct().ToList(); }

        public List<MixPagePostViewModel> Pages { get; set; }

        public string Layout { get; set; }

        public JObject Author { get; set; }

        public string BodyClass => Property<string>("body_class");

        #endregion

        #region Overrides
        public override void ExpandView(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            this.View = MixTemplateViewModel.GetTemplateByPath(Template, Specificulture, _context, _transaction).Data;
            AttributeData = MixDataHelper.LoadAdditionalData(Id.ToString(), Specificulture, MixDatabaseName.ADDITIONAL_FIELD_POST, _context, _transaction);
        }
        #endregion

        #region Expands

        public bool HasValue(string fieldName)
        {
            return AttributeData != null && AttributeData.Obj.GetValue(fieldName) != null;
        }

        public T Property<T>(string fieldName)
        {
            return MixCmsHelper.Property<T>(AttributeData?.Obj, fieldName);
        }

        #endregion Expands
    }
}
