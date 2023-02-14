using Mix.Lib.Dtos;

namespace Mix.Lib.Interfaces
{
    public interface IMixThemeExportService
    {
        public Task<string> ExportTheme(ExportThemeDto request);
    }
}
