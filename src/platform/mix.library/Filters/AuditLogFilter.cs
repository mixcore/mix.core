using Microsoft.AspNetCore.Mvc.Filters;

namespace Mix.Lib.Filters
{
    public sealed class AuditLogFilter : IResourceFilter
    {
        public AuditLogFilter()
        {
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            if (context.HttpContext.RequestServices.GetService(typeof(AuditLogService)) is AuditLogService auditLogService)
            {
                auditLogService.LogRequest(context.HttpContext);
            }
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
        }
    }
}
