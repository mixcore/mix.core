using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mix.Identity.Constants;
using Mix.Lib.Services;
using Mix.Service.Interfaces;
using System.Data.Common;

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

        public async Task InvokeAsync(HttpContext context)
        {
            // TODO: Cannot share transaction when use Sqlite
            //Dictionary<string, IDbContextTransaction> dicTransactions = new();
            //Dictionary<string, DbConnection> dicConnections = new();
            //await ShareTransaction(context, dicConnections, dicTransactions);

            var auditlogData = context.RequestServices.GetService(typeof(AuditLogDataModel)) as AuditLogDataModel;
            var auditlogService = context.RequestServices.GetService(typeof(IAuditLogService)) as AuditLogService;
            var idService = context.RequestServices.GetService(typeof(MixIdentityService)) as MixIdentityService;
            auditlogData.InitRequest(idService.GetClaim(context.User, MixClaims.Username), context);

            if (GlobalConfigService.Instance.InitStatus == InitStep.Blank)
            {
                await _next.Invoke(context);
            }
            else
            {
                await _next.Invoke(context);

                foreach (var uowType in UowInfos)
                {
                    var uowService = (IUnitOfWorkInfo)context.RequestServices.GetService(uowType);
                    await CompleteUow(uowService, context.Response.StatusCode);
                }
            }
            auditlogService.QueueRequest(auditlogData);
        }

        private async Task ShareTransaction(HttpContext context,
            Dictionary<string, DbConnection> dicConnections,
            Dictionary<string, IDbContextTransaction> dicTransactions)
        {
            if (GlobalConfigService.Instance.IsInit)
            {
                return;
            }
            try
            {
                foreach (var uowType in UowInfos)
                {

                    var uowService = (IUnitOfWorkInfo)context.RequestServices.GetService(uowType);
                    var cnn = uowService.ActiveDbContext.Database.GetConnectionString();
                    IDbContextTransaction transaction = dicTransactions.ContainsKey(cnn) ? dicTransactions[cnn] : default;
                    if (uowService.ActiveTransaction == null && transaction == null)
                    {
                        uowService.Begin();
                        dicConnections[cnn] = uowService.ActiveDbContext.Database.GetDbConnection();
                        dicTransactions[cnn] = uowService.ActiveTransaction;
                    }
                    else
                    {

                        uowService.ActiveDbContext.Database.SetDbConnection(dicConnections[cnn]);
                        await uowService.ActiveDbContext.Database.UseTransactionAsync(transaction.GetDbTransaction());
                    }

                }
            }
            catch (Exception ex)
            {
                await MixLogService.LogExceptionAsync(ex, message: "Cannot share connections");
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
