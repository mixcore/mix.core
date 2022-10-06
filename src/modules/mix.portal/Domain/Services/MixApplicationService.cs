using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Mix.Heart.Constants;
using Mix.Identity.Interfaces;
using Mix.Shared.Services;
using Mix.SignalR.Constants;
using Mix.SignalR.Hubs;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Mix.Portal.Domain.Services
{
    public class MixApplicationService : TenantServiceBase
    {
        private readonly ThemeService _themeService;
        protected MixIdentityService _mixIdentityService;
        public MixApplicationService(
            IHttpContextAccessor httpContextAccessor, 
            UnitOfWorkInfo<MixCmsContext> cmsUOW,
            MixIdentityService mixIdentityService, 
            ThemeService themeService) 
            : base(httpContextAccessor, cmsUOW)
        {
            _mixIdentityService = mixIdentityService;
            _themeService = themeService;
        }
        public async Task<MixApplicationViewModel> Install(MixApplicationViewModel app)
        {
            string name = SeoHelper.GetSEOString(app.Title);
            string appFolder = $"{MixFolders.StaticFiles}/{MixFolders.MixApplications}/{name}";
            MixFileHelper.UnZipFile(app.PackateFilePath, appFolder);


            var template = await CreateTemplate(name, appFolder);
            if (template != null)
            {
                app.SetUowInfo(_cmsUOW);
                app.BaseHref = appFolder;
                app.MixTenantId = CurrentTenant.Id;
                app.TemplateId = template.Id;
                app.CreatedBy = _mixIdentityService.GetClaim(_httpContextAccessor.HttpContext.User, MixClaims.Username);
                await app.SaveAsync();
                return app;
            }
            return null;
        }

        private async Task<MixTemplateViewModel> CreateTemplate(string name, string appFolder)
        {
            try
            {
                var indexFile = MixFileHelper.GetFileByFullName($"{appFolder}/index.html");
                Regex regex = new("((?<=src=\")|(?<=href=\"))(?!(http[^\\s]+))(.+?)(\\.+?)");

                if (indexFile.Content != null && regex.IsMatch(indexFile.Content))
                {
                    indexFile.Content = regex.Replace(indexFile.Content, $"/{appFolder}/$3$4")
                        .Replace("options['baseHref']", $"'{appFolder}'");

                    var activeTheme = await _themeService.GetActiveTheme();
                    MixTemplateViewModel template = new(_cmsUOW)
                    {
                        MixThemeId = activeTheme.Id,
                        FileName = name,
                        FileFolder = $"{MixFolders.TemplatesFolder}/{CurrentTenant.SystemName}/{activeTheme.SystemName}/{MixTemplateFolderType.Pages}",
                        FolderType = MixTemplateFolderType.Pages,
                        Extension = MixFileExtensions.CsHtml,
                        Content = indexFile.Content.Replace("@", "@@"),
                        MixTenantId = CurrentTenant.Id,
                        Scripts = string.Empty,
                        Styles = string.Empty,
                        CreatedBy = _mixIdentityService.GetClaim(_httpContextAccessor.HttpContext.User, MixClaims.Username)
                };
                    await template.SaveAsync();
                    MixFileHelper.SaveFile(indexFile);

                    return template;
                }
                throw new MixException(MixErrorStatus.Badrequest, "Invalid Application Package");
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.ServerError, ex);
            }
        }
    }
}
