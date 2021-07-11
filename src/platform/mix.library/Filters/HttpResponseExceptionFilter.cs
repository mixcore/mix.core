using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Mix.Heart.Enums;
using Mix.Heart.Exceptions;

namespace Mix.Lib.Filters
{
    public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
    {
        public int Order { get; } = int.MaxValue - 10;

        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is MixException exception)
            {
                context.Result = exception.Status switch
                {
                    MixErrorStatus.UnAuthorized => new UnauthorizedObjectResult(exception.Message)
                    {
                        StatusCode = (int)exception.Status,
                    },
                    MixErrorStatus.Forbidden => new ForbidResult()
                    {
                    },
                    MixErrorStatus.Badrequest => new BadRequestObjectResult(exception.Message)
                    {
                        StatusCode = (int)exception.Status,
                    },
                    MixErrorStatus.ServerError => new ObjectResult(exception.Message)
                    {
                        StatusCode = (int)exception.Status,
                    },
                    MixErrorStatus.NotFound => new NotFoundObjectResult(exception.Message)
                    {
                        StatusCode = (int)exception.Status,
                    },
                    _ => new ObjectResult(exception.Value)
                    {
                        StatusCode = (int)exception.Status,
                    },
                };
                context.ExceptionHandled = true;
            }
        }
    }
}
