namespace Mix.Lib.Dtos
{
    public class ImportThemeDto
    {
        public int ThemeId { get; set; }
        public bool IsIncludeAssets { get; set; } = true;

        public bool IsIncludeTemplates { get; set; } = true;

        public List<int> PostIds { get; set; } = new();

        public List<int> PageIds { get; set; } = new();

        public List<int> ModuleIds { get; set; }

        public List<int> MixDatabaseIds { get; set; }

        public List<int> MixDatabaseDataIds { get; set; }

        public List<int> ConfigurationIds { get; set; } = new();

        public List<int> LanguageIds { get; set; } = new();
    }
}
