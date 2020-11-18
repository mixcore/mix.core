using Mix.Cms.Lib.Repositories;
using Mix.Cms.Lib.Services;
using Mix.Domain.Core.ViewModels;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.ViewModels.MixThemes
{
    public class Helper
    {
        public static async Task<RepositoryResponse<string>> ExportTheme(
            int id, SiteStructureViewModel data,
            string culture, string scheme, string host)
        {
            var getTheme = await ReadViewModel.Repository.GetSingleModelAsync(
                 theme => theme.Id == id).ConfigureAwait(false);

            //path to temporary folder
            string tempPath = $"wwwroot/Exports/Themes/{getTheme.Data.Name}/temp";
            string outputPath = $"Exports/Themes/{getTheme.Data.Name}";
            data.ThemeName = getTheme.Data.Name;
            data.Specificulture = culture;
            var result = data.ExportSelectedItemsAsync();
            if (result.IsSucceed)
            {
                string domain = MixService.GetConfig<string>("Domain");
                string accessFolder = $"{MixConstants.Folder.FileFolder}/{MixConstants.Folder.TemplatesAssetFolder}/{getTheme.Data.Name}/assets";
                string content = JObject.FromObject(data).ToString()
                    .Replace(accessFolder, "[ACCESS_FOLDER]")
                    .Replace($"/{culture}/", "/[CULTURE]/");
                if (!string.IsNullOrEmpty(domain))
                {
                    content = content.Replace(domain, string.Empty);
                }
                string filename = $"schema";
                var file = new FileViewModel()
                {
                    Filename = filename,
                    Extension = ".json",
                    FileFolder = $"{tempPath}/Data",
                    Content = content
                };

                // Delete Existing folder
                FileRepository.Instance.DeleteFolder(outputPath);
                // Copy current templates file
                FileRepository.Instance.CopyDirectory($"{getTheme.Data.TemplateFolder}", $"{tempPath}/Templates");
                // Copy current assets files
                FileRepository.Instance.CopyDirectory($"{getTheme.Data.AssetFolder}", $"{tempPath}/Assets");
                // Copy current uploads files
                FileRepository.Instance.CopyDirectory($"{getTheme.Data.UploadsFolder}", $"{tempPath}/Uploads");
                // Save Site Structures
                FileRepository.Instance.SaveFile(file);

                // Zip to [theme_name].zip ( wwwroot for web path)
                string filePath = FileRepository.Instance.ZipFolder($"{tempPath}", outputPath, $"{getTheme.Data.Name}-{Guid.NewGuid()}");

                // Delete temp folder
                FileRepository.Instance.DeleteWebFolder($"{outputPath}/Assets");
                FileRepository.Instance.DeleteWebFolder($"{outputPath}/Uploads");
                FileRepository.Instance.DeleteWebFolder($"{outputPath}/Templates");
                FileRepository.Instance.DeleteWebFolder($"{outputPath}/Data");

                return new RepositoryResponse<string>()
                {
                    IsSucceed = !string.IsNullOrEmpty(outputPath),
                    Data = $"{scheme}://{host}/{filePath}"
                };
            }
            else
            {
                return result;
            }
        }
    }
}
