namespace Mix.Lib.ViewModels
{
    public sealed class SiteDataViewModel
    {
        public bool IsValid { get; set; }
        public List<string> InvalidDatabaseNames { get; set; } = new();
        public List<string> Errors { get; set; } = new();

        public string CreatedBy { get; set; }

        public string Specificulture { get; set; }

        public string ThemeName { get; set; }

        public string ThemeSystemName { get; set; }

        public int ThemeId { get; set; }

        #region Main Objects

        public List<MixPost> Posts { get; set; } = new();

        public List<MixPage> Pages { get; set; } = new();

        public List<MixModule> Modules { get; set; }

        public List<MixDatabaseContext> MixDatabaseContexts { get; set; } = new();
        public List<MixDatabase> MixDatabases { get; set; } = new();
        public List<MixDatabaseRelationship> MixDatabaseRelationships { get; set; } = new();

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

        public List<MixDbModel> MixDbModels { get; set; } = new();

        public List<MixModuleData> ModuleDatas { get; set; } = new();

        public List<MixPagePostAssociation> PagePosts { get; set; } = new();
        public List<MixPostPostAssociation> PostPosts { get; set; } = new();
        public List<MixPageModuleAssociation> PageModules { get; set; } = new();
        public List<MixModulePostAssociation> ModulePosts { get; set; } = new();
        public List<MixUrlAlias> MixUrlAliases { get; set; } = new();

        #endregion

        public SiteDataViewModel()
        {

        }
    }

    public sealed class MixDbModel
    {
        public string DatabaseName { get; set; }
        public JArray Data { get; set; }
    }
}