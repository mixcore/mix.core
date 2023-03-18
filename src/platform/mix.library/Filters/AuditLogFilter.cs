using Microsoft.AspNetCore.Mvc.Filters;
using Mix.Service.Interfaces;
using Mix.Service.Services;

namespace Mix.Lib.Filters
{
    public class AuditLogFilter : IResourceFilter
    {
        private Guid _id;
        public AuditLogFilter()
        {
            
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            _id = Guid.NewGuid();
            if (context.HttpContext.RequestServices.GetService(typeof(IAuditLogService)) is IAuditLogService auditLogService)
            {
                auditLogService.LogRequest(_id, context.HttpContext);
            }
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            if (context.HttpContext.RequestServices.GetService(typeof(IAuditLogService)) is IAuditLogService auditLogService)
            {
                auditLogService.LogResponse(_id, context.HttpContext.Response, context.Exception);
            }
        }
    }
}
