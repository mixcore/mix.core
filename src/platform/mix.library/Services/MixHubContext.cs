using Microsoft.AspNetCore.Http;

namespace Mix.Lib.Services
{
    public sealed class MixHubContext(IHttpContextAccessor contextAccessor)
    {
        public string? ConnectionId
        {
            get
            {
                if (contextAccessor?.HttpContext?.Request == null)
                {
                    return null;
                }

                return contextAccessor.HttpContext.Request.Headers["hub-connection-id"];
            }
            private set { }
        }
    }
}
