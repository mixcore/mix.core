using Microsoft.AspNetCore.Mvc.Filters;
using Mix.Lib.Services;

namespace Mix.Lib.Filters
{
    public sealed class AuditLogFilter : IResourceFilter
    {
        public AuditLogFilter()
        {
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            var _auditLogService = context.HttpContext.RequestServices.
            GetService(typeof(AuditLogService)) as AuditLogService;
            _auditLogService.LogRequest(context.HttpContext);
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
        }
    }
}
