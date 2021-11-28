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

        public List<int> CultureIds { get; set; }

        public SelectedExport Content { get; set; }
        public SelectedExport Associations { get; set; }
    }

    public class SelectedExport
    {
        public List<int> PageIds { get; set; }
        public List<int> PageContentIds { get; set; }

        public List<int> PostIds { get; set; }
        public List<int> PostContentIds { get; set; }

        public List<int> ModuleIds { get; set; }
        public List<int> ModuleContentIds { get; set; }

        public List<int> MixDatabaseIds { get; set; }
    }
}
