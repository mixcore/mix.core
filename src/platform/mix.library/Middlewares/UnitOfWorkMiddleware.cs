using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mix.Lib.Middlewares
{
    public class UnitOfWorkMiddleware
    {
        private readonly RequestDelegate next;
        public UnitOfWorkMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context, 
            [FromServices] UnitOfWorkInfo<MixCmsContext> cmsUOW,
            [FromServices] UnitOfWorkInfo<MixCacheDbContext> cacheUOW
            )
        {
            if (GlobalConfigService.Instance.InitStatus == InitStep.Blank)
            {
                await next.Invoke(context);
            }
            else
            {
                await next.Invoke(context);
                
                await CompleteUOW(cmsUOW, context.Response.StatusCode);
                await CompleteUOW(cacheUOW, context.Response.StatusCode);

            }
        }

        private async Task CompleteUOW(UnitOfWorkInfo _cmsUOW, int statusCode)
        {
            if (_cmsUOW.ActiveTransaction != null)
            {
                if (Enum.IsDefined(typeof(MixErrorStatus), statusCode))
                {
                    await _cmsUOW.RollbackAsync();
                }
                else
                {
                    await _cmsUOW.CompleteAsync();
                }
            }
            _ = _cmsUOW.ActiveDbContext?.DisposeAsync();
        }
    }
}
