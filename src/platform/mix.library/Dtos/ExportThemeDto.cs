namespace Mix.Lib.Dtos
{
    public class ExportThemeDto
    {
        public bool IsExportAll { get; set; }

        public bool IsIncludeAssets { get; set; } = true;

        public bool IsIncludeTemplates { get; set; } = true;

        public bool IsIncludeConfigurations { get; set; } = true;

        public bool IsIncludePermissions { get; set; } = true;

        public int ThemeId { get; set; }

        public string CreatedBy { get; set; }

        public string Specificulture { get; set; }

        public ExportData ExportData { get; set; }
    }

    public class ExportData
    {
        public List<int> PageIds { get; set; }

        public List<int> ModuleIds { get; set; }

        public List<int> DatabaseIds { get; set; }
    }
}
