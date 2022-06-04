using Microsoft.AspNetCore.Http;
using Mix.Lib.Services;

namespace Mix.Lib.Middlewares
{
    public class AuditLogMiddleware
    {
        private readonly RequestDelegate next;
        protected readonly AuditLogService _auditLogService;
        public AuditLogMiddleware(
            RequestDelegate next, 
            AuditLogService auditLogService)
        {
            this.next = next;
            _auditLogService = auditLogService;
        }

        public async Task Invoke(
            HttpContext context)
        {
            if (GlobalConfigService.Instance.InitStatus == InitStep.Blank)
            {
                await next.Invoke(context);
            }
            else
            {
                _auditLogService.LogRequest(context);
                await next.Invoke(context);
            }
        }
    }
}
