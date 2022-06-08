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
            [FromServices] GenericUnitOfWorkInfo<MixCmsContext> cmsUOW,
            [FromServices] GenericUnitOfWorkInfo<MixCacheDbContext> cacheUOW
            )
        {
            if (GlobalConfigService.Instance.InitStatus == InitStep.Blank)
            {
                await next.Invoke(context);
            }
            else
            {
                await next.Invoke(context);
                await CompleteUOW(cmsUOW);
                await CompleteUOW(cacheUOW);
            }
        }

        private async Task CompleteUOW(UnitOfWorkInfo _cmsUOW)
        {
            if (_cmsUOW.ActiveTransaction != null)
            {
                await _cmsUOW.CompleteAsync();
            }
            _ = _cmsUOW.ActiveDbContext?.DisposeAsync();
        }
    }
}
