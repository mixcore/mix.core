using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Mix.Log.Lib.Models;

namespace Mix.Lib.Filters
{
    public sealed class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
    {
        private bool _isProduction;

        public HttpResponseExceptionFilter(bool isProduction)
        {
            _isProduction = isProduction;
        }

        public int Order { get; } = int.MaxValue - 10;

        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
            {

                var auditLogData = context.HttpContext.RequestServices.GetService(typeof(AuditLogDataModel)) as AuditLogDataModel;
                auditLogData.Exception = ReflectionHelper.ParseObject(context.Exception);

                var result = new ExceptionResponseResult(context.Exception.GetType().Name, context.Exception.Message, _isProduction ? null : context.Exception.StackTrace);
                if (context.Exception is MixException exception)
                {
                    result.Errors = exception.Errors;
                    context.Result = exception.Status switch
                    {
                        MixErrorStatus.UnAuthorized => new UnauthorizedObjectResult(result)
                        {
                            StatusCode = (int)exception.Status,
                        },
                        MixErrorStatus.Forbidden => new ForbidResult()
                        {
                        },
                        MixErrorStatus.Badrequest => new BadRequestObjectResult(result)
                        {
                            StatusCode = (int)exception.Status,
                        },
                        MixErrorStatus.ServerError => new ObjectResult(result)
                        {
                            StatusCode = (int)exception.Status,
                        },
                        MixErrorStatus.NotFound => new NotFoundObjectResult(result)
                        {
                            StatusCode = (int)exception.Status,
                        },
                        _ => new ObjectResult(result)
                        {
                            StatusCode = (int)exception.Status,
                        },
                    };
                    context.ExceptionHandled = true;
                }
                context.Result = new BadRequestObjectResult(result);
                context.ExceptionHandled = true;
            }

        }
    }

    public class ExceptionResponseResult(string code, string message, string stackTrace)
    {
        public string Code { get; set; } = code;
        public string Message { get; set; } = message; // TODO: must be ignored in production
        public string[] Errors { get; set; }
        public string StackTrace { get; set; } = stackTrace; // TODO: must be ignored in production
    }
}
