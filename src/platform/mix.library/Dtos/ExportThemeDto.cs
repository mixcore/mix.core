using Mix.Database.Entities.Cms;
using System.Collections.Generic;

namespace Mix.Lib.Dtos
{
    public class ExportThemeDto
    {
        public bool IsExportAll { get; set; }

        public bool IsIncludeAssets { get; set; } = true;

        public bool IsIncludeTemplates { get; set; } = true;

        public bool IsIncludeConfigurations { get; set; } = true;

        public bool IsIncludePermissions { get; set; } = true;

        public string CreatedBy { get; set; }

        public List<MixPost> Posts { get; set; } = new();

        public List<MixPage> Pages { get; set; } = new();

        public List<MixModule> Modules { get; set; }

        public List<MixDatabase> MixDatabases { get; set; }

        public List<MixViewTemplate> Templates { get; set; } = new();

        public List<MixConfiguration> Configurations { get; set; } = new();

        public List<MixLanguage> Languages { get; set; } = new();

        public ExportData ExportData { get; set; }
    }

    public class ExportData
    {
        public List<int> PageIds { get; set; }

        public List<int> ModuleIds { get; set; }

        public List<int> DatabaseIds { get; set; }
    }
}
