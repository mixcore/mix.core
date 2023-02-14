using Microsoft.AspNetCore.Mvc.Filters;
using Mix.Service.Interfaces;

namespace Mix.Lib.Filters
{
    public sealed class AuditLogFilter : IResourceFilter
    {
        public AuditLogFilter()
        {
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            if (context.HttpContext.RequestServices.GetService(typeof(IAuditLogService)) is IAuditLogService auditLogService)
            {
                auditLogService.LogRequest(context.HttpContext);
            }
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
        }
    }
}
