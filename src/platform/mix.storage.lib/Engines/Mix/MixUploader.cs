using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Mix.Storage.Lib.Engines.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Storage.Lib.Engines.Mix
{
    public class MixUploader : UploaderBase
    {
        public MixUploader(IHttpContextAccessor httpContext, IConfiguration configuration, UnitOfWorkInfo<MixCmsContext> cmsUOW) 
            : base(httpContext, configuration, cmsUOW)
        {
        }

        public override async Task<string?> Upload(IFormFile file, string? themeName, string? createdBy)
        {
            var folder = $"{MixFolders.StaticFiles}/{_tenantName}/{themeName}/{MixFolders.UploadsFolder}/{DateTime.Now.ToString("yyyy-MMM")}";
            var result = MixFileHelper.SaveFile(file, folder);
            if (!string.IsNullOrEmpty(result))
            {
                return $"{GlobalConfigService.Instance.Domain}/{folder}/{result}";
            }
            return default;
        }
    }
}
