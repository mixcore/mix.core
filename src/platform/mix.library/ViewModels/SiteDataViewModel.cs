using Mix.Lib.Dtos;

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

        public List<MixDatabase> MixDatabases { get; set; } = new();

        public List<MixTemplate> Templates { get; set; } = new();

        public List<MixConfiguration> Configurations { get; set; } = new();

        public List<MixLanguage> Languages { get; set; } = new();

        #endregion

        #region Association Objects

        public List<MixDatabaseColumn> MixDatabaseColumns { get; set; } = new();

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

        public SiteDataViewModel()
        {

        }
    }
}