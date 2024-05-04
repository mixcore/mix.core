using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Mix.Lib.Attributes
{
    public class CacheWebApiAttribute : ActionFilterAttribute
    {
        public int Duration { get; set; }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            context.HttpContext.Response.Headers.Remove("Cache-Control");
            context.HttpContext.Response.Headers.Remove("Pragma");
            context.HttpContext.Response.Headers.Append("Cache-Control",
                string.Format("public,max-age={0}", Duration));
        }
    }
}
