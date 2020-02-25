using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Mix.Cms.Lib.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.MiddleWares
{
    public class IpRestrictionMiddleware
    {
        public readonly RequestDelegate Next;
        public readonly IpSecuritySettings IpSecuritySettings;

        public IpRestrictionMiddleware(RequestDelegate next, IOptions<IpSecuritySettings> ipSecuritySettings)
        {
            Next = next;
            IpSecuritySettings = ipSecuritySettings.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            if (MixService.GetIpConfig<bool>("IsRetrictIp"))
            {
                var ipAddress = (string)context.Connection.RemoteIpAddress?.ToString();
                string allowedIps = MixService.GetIpConfig<string>("AllowedIps");
                string exceptIps = MixService.GetIpConfig<string>("ExceptIps");
                string remoteIp = context.Connection?.RemoteIpAddress?.ToString();
                if (
                    // allow localhost
                    (allowedIps != "*" && !allowedIps.Contains(remoteIp))
                    || (exceptIps.Contains(remoteIp))
                    )
                {
                    context.Response.StatusCode = 403;
                    return;
                }
            }
            await Next(context);
        }
    }

    public class IpSecuritySettings
    {
        public bool IsRetrictIp { get; set; }
        public string AllowedPortalIps { get; set; }
        public string AllowedIps { get; set; }
        public string ExceptIps { get; set; }

        public List<string> AllowedIPsList {
            get { return !string.IsNullOrEmpty(AllowedIps) ? AllowedIps.Split(',').ToList() : new List<string>(); }
        }
    }
}