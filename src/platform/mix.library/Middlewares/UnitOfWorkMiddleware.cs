using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mix.Database.Entities.Account;

namespace Mix.Lib.Middlewares
{
    public class UnitOfWorkMiddleware
    {
        private readonly RequestDelegate next;
        private static List<Type> UowInfos = new List<Type>();
        public UnitOfWorkMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
        {
            this.next = next;
        }

        public static void AddUnitOfWork<T>()
            where T : IUnitOfWorkInfo
        {
            UowInfos.Add(typeof(T));
        }

        public async Task Invoke(HttpContext context,
            [FromServices] UnitOfWorkInfo<MixCmsContext> cmsUOW,
            [FromServices] UnitOfWorkInfo<MixCmsAccountContext> accountUOW,
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
                await CompleteUOW(accountUOW, context.Response.StatusCode);
                await CompleteUOW(cacheUOW, context.Response.StatusCode);
                foreach (var uowType in UowInfos)
                {
                    IUnitOfWorkInfo srv = (IUnitOfWorkInfo)context.RequestServices.GetService(uowType);
                    await CompleteUOW(srv, context.Response.StatusCode);
                }
            }
        }

        private async Task CompleteUOW(IUnitOfWorkInfo _cmsUOW, int statusCode)
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
            _ = _cmsUOW.DisposeAsync();
        }
    }
}
