namespace Mix.Lib.Dtos
{
    public class ImportThemeDto
    {
        public bool IsIncludeAssets { get; set; } = true;

        public bool IsIncludeTemplates { get; set; } = true;

        public List<MixPost> Posts { get; set; } = new();

        public List<MixPage> Pages { get; set; } = new();

        public List<MixModule> Modules { get; set; }

        public List<MixDatabase> MixDatabases { get; set; }

        public List<MixConfiguration> Configurations { get; set; } = new();

        public List<MixLanguage> Languages { get; set; } = new();
    }
}
