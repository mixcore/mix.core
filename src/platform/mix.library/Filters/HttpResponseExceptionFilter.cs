using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Mix.Heart.Exceptions;

namespace Mix.Lib.Filters
{
    public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
    {
        public int Order { get; } = int.MaxValue - 10;

        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is MixHttpResponseException exception)
            {
                switch (exception.Status)
                {
                    case Heart.Enums.MixErrorStatus.UnAuthorized:
                        context.Result = new UnauthorizedObjectResult(exception.Value)
                        {
                            StatusCode = (int)exception.Status,
                        };
                        break;
                    case Heart.Enums.MixErrorStatus.Forbidden:
                        context.Result = new ForbidResult()
                        {
                        };
                        break;
                    case Heart.Enums.MixErrorStatus.Badrequest:
                        context.Result = new BadRequestObjectResult(exception.Value)
                        {
                            StatusCode = (int)exception.Status,
                        };
                        break;
                    case Heart.Enums.MixErrorStatus.ServerError:
                        context.Result = new ObjectResult(exception.Value)
                        {
                            StatusCode = (int)exception.Status,
                        };
                        break;
                    default:
                        break;
                }                
                context.ExceptionHandled = true;
            }
        }
    }
}
