using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Mix.Lib.Filters
{
    public sealed class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
    {
        public int Order { get; } = int.MaxValue - 10;

        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is MixException exception)
            {
                context.Result = exception.Status switch
                {
                    MixErrorStatus.UnAuthorized => new UnauthorizedObjectResult(exception.Errors)
                    {
                        StatusCode = (int)exception.Status,
                    },
                    MixErrorStatus.Forbidden => new ForbidResult()
                    {
                    },
                    MixErrorStatus.Badrequest => new BadRequestObjectResult(exception.Errors)
                    {
                        StatusCode = (int)exception.Status,
                    },
                    MixErrorStatus.ServerError => new ObjectResult(exception.Errors)
                    {
                        StatusCode = (int)exception.Status,
                    },
                    MixErrorStatus.NotFound => new NotFoundObjectResult(exception.Errors)
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
            else if (context.Exception != null)
            {
                context.Result = new BadRequestObjectResult(context.Exception.Message?.Split('\n'));
            }
            context.ExceptionHandled = true;
        }
    }
}
