using Mix.Lib.Dtos;

namespace Mix.Lib.Services
{
    public interface IMixThemeExportService
    {
        Task<string> ExportTheme(ExportThemeDto request);
    }
}
