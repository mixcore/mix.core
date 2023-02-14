using Microsoft.AspNetCore.Http;

namespace Mix.Lib.Interfaces
{
    public interface IMixThemeImportService
    {
        public Task DownloadThemeAsync(JObject theme, IProgress<int> progress, HttpService httpService, string folder = MixFolders.ThemePackage);
        public Task<SiteDataViewModel> LoadSchema();
        public void ExtractTheme(IFormFile themeFile);
        public Task<SiteDataViewModel> ImportSelectedItemsAsync(SiteDataViewModel siteData);
    }
}
