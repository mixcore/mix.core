using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mix.Database.Entities.Account;

namespace Mix.Lib.Middlewares
{
    public class UnitOfWorkMiddleware
    {
        private readonly RequestDelegate _next;
        private static readonly List<Type> UowInfos = new();
        public UnitOfWorkMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public static void AddUnitOfWork<T>() where T : IUnitOfWorkInfo
        {
            UowInfos.Add(typeof(T));
        }

        public async Task InvokeAsync(
            HttpContext context,
            [FromServices] UnitOfWorkInfo<MixCmsContext> cmsUow,
            [FromServices] UnitOfWorkInfo<MixCmsAccountContext> accountUow,
            [FromServices] UnitOfWorkInfo<MixCacheDbContext> cacheUow)
        {
            if (GlobalConfigService.Instance.InitStatus == InitStep.Blank)
            {
                await _next.Invoke(context);
            }
            else
            {
                await _next.Invoke(context);

                await CompleteUow(cmsUow, context.Response.StatusCode);
                await CompleteUow(accountUow, context.Response.StatusCode);
                await CompleteUow(cacheUow, context.Response.StatusCode);

                foreach (var uowType in UowInfos)
                {
                    var uowService = (IUnitOfWorkInfo)context.RequestServices.GetService(uowType);
                    await CompleteUow(uowService, context.Response.StatusCode);
                }
            }
        }

        private async Task CompleteUow(IUnitOfWorkInfo cmsUow, int statusCode)
        {
            if (cmsUow.ActiveTransaction != null)
            {
                if (Enum.IsDefined(typeof(MixErrorStatus), statusCode))
                {
                    await cmsUow.RollbackAsync();
                }
                else
                {
                    await cmsUow.CompleteAsync();
                }
            }

            cmsUow.Dispose();
        }
    }
}
