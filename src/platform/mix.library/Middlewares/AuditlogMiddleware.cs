using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Mix.Auth.Constants;
using Mix.Lib.Extensions;
using Mix.Lib.Services;
using Mix.Log.Lib.Interfaces;
using Mix.Log.Lib.Models;
using Mix.Shared.Models.Configurations;
using System.Text;

namespace Mix.Lib.Middlewares
{
    public class AuditlogMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IAuditLogService _auditlogService;
        private readonly IConfiguration _configuration;
        private readonly GlobalSettingsModel _globalConfig;
        public AuditlogMiddleware(RequestDelegate next, IConfiguration configuration, IAuditLogService auditlogService)
        {
            _configuration = configuration;
            _globalConfig = _configuration.Get<GlobalSettingsModel>();
            _next = next;
            _auditlogService = auditlogService;
        }

        public async Task InvokeAsync(HttpContext context, AuditLogDataModel auditLogData)
        {
            var logConfigurations = _configuration.GetSection(MixAppSettingsSection.Log).Get<LogConfigurations>();
            var isLog = CheckAuditLogPath(context.Request.Path);
            if (!isLog)
            {
                await _next(context);
            }

            else
            {
                //Copy a pointer to the original response body stream
                await LogRequest(context, auditLogData);

                //Copy a pointer to the original response body stream
                var originalBodyStream = context.Response.Body;

                if (!logConfigurations.EnableAuditLogResponse)
                {
                    if (logConfigurations.EnableAuditLog)
                    {
                        _auditlogService.QueueRequest(auditLogData);
                    }
                    await _next(context);
                }
                else
                {
                    //POST a new memory stream...
                    using (var responseBody = new MemoryStream())
                    {
                        //Continue down the Middleware pipeline, eventually returning to this class
                        await _next(context);

                        //Format the response from the server
                        await LogResponse(context, auditLogData);
                        _auditlogService.QueueRequest(auditLogData);
                    }
                }
            }

            auditLogData.StatusCode = context.Response.StatusCode;
            if (logConfigurations.IsLogStream && isLog)
            {
                await _auditlogService.LogStream(auditLogData.Endpoint, auditLogData.Exception ?? auditLogData.Body, isSuccess: auditLogData.StatusCode < 300);
            }
        }

        private bool CheckAuditLogPath(string path)
        {
            return !_configuration.IsInit() && (path.IndexOf("/api") >= 0 && path.IndexOf("audit-log") < 0 && path.IndexOf("queue-log") < 0);
        }

        private async Task LogRequest(HttpContext context, AuditLogDataModel auditLogData)
        {
            var idService = context.RequestServices.GetService(typeof(MixIdentityService)) as MixIdentityService;
            var request = await FormatRequest(context.Request);
            auditLogData.CreatedBy = idService.GetClaim(context.User, MixClaims.UserName);
            auditLogData.RequestIp = context.Request.HttpContext?.Connection?.RemoteIpAddress?.ToString();
            auditLogData.Endpoint = context.Request.Path;
            auditLogData.Method = context.Request.Method;
            auditLogData.QueryString = context.Request.QueryString.ToString();
            auditLogData.Body = request.IsJsonString() ? JObject.Parse(request) : new JObject(new JProperty("data", request));
        }

        private async Task LogResponse(HttpContext context, AuditLogDataModel auditLogData)
        {
            if (context.Response.Body.CanSeek)
            {
                var response = await FormatResponse(context.Response);
                auditLogData.StatusCode = context.Response.StatusCode;
                auditLogData.Response = response.IsJsonString() ? JObject.Parse(response) : new JObject(new JProperty("data", response));
            }
        }

        private async Task<string> FormatResponse(HttpResponse response)
        {
            //We need to read the response stream from the beginning...
            response.Body.Seek(0, SeekOrigin.Begin);

            var requestReader = new StreamReader(response.Body);
            var bodyAsText = await requestReader.ReadToEndAsync();
            response.Body.Position = 0;
            return bodyAsText;
        }

        private async Task<string> FormatRequest(HttpRequest request)
        {
            if (request.ContentLength > 0)
            {
                //This line allows us to set the reader for the request back at the beginning of its stream.
                request.EnableBuffering();

                var requestReader = new StreamReader(request.Body);
                var bodyAsText = await requestReader.ReadToEndAsync();
                request.Body.Position = 0;
                return bodyAsText;
            }
            return string.Empty;
        }
    }
}