using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Lib.Extensions;
using Mix.Shared.Models.Configurations;
using System.Data.Common;

namespace Mix.Lib.Middlewares
{
    public class UnitOfWorkMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        private static readonly List<Type> UowInfos = [];
    private readonly GlobalSettingsModel _globalConfig = configuration.Get<GlobalSettingsModel>();

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

        if (configuration.GetValue<InitStep>("InitStatus") == InitStep.Blank || MixCmsHelper.CheckStaticFileRequest(context.Request.Path))
        {
            await next.Invoke(context);
        }
        else
        {
            await next.Invoke(context);

            foreach (var uowType in UowInfos)
            {
                var uowService = (IUnitOfWorkInfo)context.RequestServices.GetService(uowType);
                await CompleteUow(uowService, context.Response.StatusCode);
            }
        }
    }

    private async Task ShareTransaction(
        IConfiguration configuration,
        HttpContext context,
        Dictionary<string, DbConnection> dicConnections,
        Dictionary<string, IDbContextTransaction> dicTransactions)
    {
        if (configuration.IsInit())
        {
            return;
        }
        try
        {
            foreach (var uowType in UowInfos)
            {

                var uowService = (IUnitOfWorkInfo)context.RequestServices.GetService(uowType);
                var cnn = uowService.ActiveDbContext.Database.GetConnectionString();
                IDbContextTransaction transaction = dicTransactions.TryGetValue(cnn, out IDbContextTransaction value) ? value : default;
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

    private static async Task CompleteUow(IUnitOfWorkInfo uow, int statusCode)
    {
        if (uow.ActiveTransaction != null)
        {
            if (Enum.IsDefined(typeof(MixErrorStatus), statusCode))
            {
                await uow.RollbackAsync();
            }
            else
            {
                await uow.CompleteAsync();
            }
        }

        // Ensure connection is closed/disposed
        try
        {
            var dbContext = uow.ActiveDbContext;
            if (dbContext != null)
            {
                var connection = dbContext.Database.GetDbConnection();
                await SafeCloseConnectionAsync(connection);
            }
        }
        catch (Exception ex)
        {
            await MixLogService.LogExceptionAsync(ex, message: "Error closing DB connection in UnitOfWorkMiddleware");
        }

        uow.Dispose();
    }

    private static async Task SafeCloseConnectionAsync(DbConnection? connection)
    {
        if (connection == null) return;
        try
        {
            if (connection.State != System.Data.ConnectionState.Closed)
            {
                await connection.CloseAsync();
            }
            connection.Dispose();
        }
        catch (Exception ex)
        {
            await MixLogService.LogExceptionAsync(ex, message: "Error disposing DB connection");
        }
    }
}
}
