using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mix.Database.Entities.Account;
using Mix.Shared.Services;
using Mix.Universal.Lib.Entities;

namespace Mix.Universal.Lib.Middlewares
{
    public class MixUniversalUnitOfWorkMiddleware
    {
        private readonly RequestDelegate next;
        public MixUniversalUnitOfWorkMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context,
            [FromServices] UnitOfWorkInfo<MixUniversalDbContext> cmsUOW
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
