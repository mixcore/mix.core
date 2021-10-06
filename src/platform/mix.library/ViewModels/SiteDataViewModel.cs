using Mix.Database.Entities.Cms;
using Mix.Lib.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mix.Lib.ViewModels
{
    public class SiteDataViewModel
    {
        public string CreatedBy { get; set; }

        public string Specificulture { get; set; }

        public string ThemeName { get; set; }

        #region Main Objects

        public List<MixPost> Posts { get; set; } = new();

        public List<MixPage> Pages { get; set; } = new();

        public List<MixModule> Modules { get; set; }

        public List<MixDatabase> Databases { get; set; } = new();

        public List<MixViewTemplate> Templates { get; set; } = new();

        public List<MixConfiguration> Configurations { get; set; } = new();

        public List<MixLanguage> Languages { get; set; } = new();

        #endregion

        #region Data Objects

        public List<MixDatabaseColumn> DatabaseColumns { get; set; } = new();

        public List<MixPostContent> PostContents { get; set; } = new();

        public List<MixPageContent> PageContents { get; set; } = new();

        public List<MixModuleContent> ModuleContents { get; set; }

        public List<MixConfigurationContent> ConfigurationContents { get; set; }

        public List<MixLanguageContent> LanguageContents { get; set; }

        public List<MixData> Datas { get; set; } = new();

        public List<MixDataContent> DataContents { get; set; } = new();

        public List<MixDataContentValue> DataContentValues { get; set; } = new();

        public List<MixDataContentAssociation> DataContentAssociations { get; set; } = new();

        public List<MixModuleData> ModuleDatas { get; set; } = new();

        public List<MixPagePostAssociation> PagePosts { get; set; } = new();

        public List<MixPageModuleAssociation> PageModules { get; set; } = new();

        public List<MixModulePostAssociation> ModulePosts { get; set; } = new();

        public List<MixUrlAlias> MixUrlAliases { get; set; } = new();

        #endregion

        #region Internals

        internal List<int> PageIds { get; set; }
        internal List<int> ModuleIds { get; set; }
        internal List<int> PostIds { get; set; }
        internal List<int> PageContentIds { get; set; }
        internal List<int> ModuleContentIds { get; set; }
        internal List<int> PostContentIds { get; set; }
        internal List<int> DatabaseIds { get; set; }
        internal List<int> ConfigurationIds { get; set; }
        internal List<int> LanguageIds { get; set; }
        internal List<Guid> DataIds { get; set; } = new();
        internal List<Guid> DataContentIds { get; set; } = new();
        internal List<Guid> DataContentAssociationIds { get; set; } = new();

        #endregion

        public SiteDataViewModel(ExportThemeDto requestDto)
        {
            if (!requestDto.IsExportAll)
            {
                Templates = requestDto.Templates;
                Pages = requestDto.Pages;
                Posts = requestDto.Posts;
                Modules = requestDto.Modules;
                Databases = requestDto.MixDatabases;
                Configurations = requestDto.Configurations;
                Languages = requestDto.Languages;

                PageIds = Pages.Select(m => m.Id).ToList();
                PostIds = Posts.Select(m => m.Id).ToList();
                ModuleIds = Modules.Select(m => m.Id).ToList();
                DatabaseIds = Databases.Select(m => m.Id).ToList();
                ConfigurationIds = Configurations.Select(m => m.Id).ToList();
                LanguageIds = Languages.Select(m => m.Id).ToList();
            }
        }

        public SiteDataViewModel(ImportThemeDto dto)
        {

        }
    }
}