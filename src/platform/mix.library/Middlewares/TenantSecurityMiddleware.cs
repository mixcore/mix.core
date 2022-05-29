using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Mix.Lib.Models;
using Mix.Shared.Commands;
using Mix.Shared.Models;
using System.Text;

namespace Mix.Lib.Middlewares
{
    public class TenantSecurityMiddleware
    {
        private readonly RequestDelegate next;
        protected readonly IQueueService<MessageQueueModel> _queueService;
        public TenantSecurityMiddleware(RequestDelegate next, IQueueService<MessageQueueModel> queueService)
        {
            this.next = next;
            _queueService = queueService;
        }

        public async Task Invoke(
            HttpContext context,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            MixCmsContext cmsContext)
        {
            if (GlobalConfigService.Instance.InitStatus == InitStep.Blank)
            {
                await next.Invoke(context);
            }
            else
            {
                //LogRequest(context);
                if (MixTenantRepository.Instance.AllTenants == null)
                {
                    var uow = new UnitOfWorkInfo(cmsContext);
                    MixTenantRepository.Instance.AllTenants = await MixTenantViewModel.GetRepository(uow).GetAllAsync(m => true);
                }
                if (!context.Session.GetInt32(MixRequestQueryKeywords.MixTenantId).HasValue)
                {
                    context.Session.SetInt32(
                        MixRequestQueryKeywords.MixTenantId,
                        MixTenantRepository.Instance.GetCurrentTenant(context.Request.Headers.Host).Id);
                }
                await next.Invoke(context);
            }
        }

        private void LogRequest(HttpContext context)
        {
            context.Request.EnableBuffering();
            var request = new ParsedRequestModel(
                $"{context.Request.HttpContext.Connection.RemoteIpAddress} - {context.Request.Headers.Referer}",
                context.Request.Path,
                context.Request.Method,
                GetBody(context.Request)
                );
            var cmd = new LogAuditLogCommand(context.User.Identity?.Name, request);
            _queueService.PushQueue(MixQueueTopics.MixBackgroundTasks, MixQueueActions.AuditLog, cmd);
            context.Request.Body.Seek(0, SeekOrigin.Begin);
        }



        private string GetBody(HttpRequest request)
        {
            var bodyStr = "";
            // Arguments: Stream, Encoding, detect encoding, buffer size 
            // AND, the most important: keep stream opened
            using (StreamReader reader
                      = new StreamReader(request.BodyReader.AsStream(), Encoding.UTF8, true, 1024, true))
            {
                bodyStr = reader.ReadToEndAsync().GetAwaiter().GetResult();
            }

            return bodyStr;
        }
    }
}
