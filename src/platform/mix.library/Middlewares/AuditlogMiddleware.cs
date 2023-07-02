using Microsoft.AspNetCore.Http;
using Mix.Identity.Constants;
using Mix.Lib.Services;
using Mix.Service.Interfaces;
using System.Text;

namespace Mix.Lib.Middlewares
{
    public class AuditlogMiddleware
    {
        private readonly RequestDelegate _next;
        private AuditLogService _auditlogService;
        private AuditLogDataModel _auditlogData;
        public AuditlogMiddleware(RequestDelegate next)
        {
            _next = next;
            _auditlogData = new();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (CheckAuditLogPath(context.Request.Path))
            {
                //Copy a pointer to the original response body stream
                _auditlogService = context.RequestServices.GetService(typeof(IAuditLogService)) as AuditLogService;

                await LogRequest(context);


                //Copy a pointer to the original response body stream
                var originalBodyStream = context.Response.Body;

                //Create a new memory stream...
                using (var responseBody = new MemoryStream())
                {
                    //...and use that for the temporary response body
                    context.Response.Body = responseBody;

                    //Continue down the Middleware pipeline, eventually returning to this class
                    await _next(context);

                    //Format the response from the server
                    await LogResponse(context);

                    //Copy the contents of the new memory stream (which contains the response) to the original stream, which is then returned to the client.
                    await responseBody.CopyToAsync(originalBodyStream);

                    _auditlogService.QueueRequest(_auditlogData);
                }
            }
            else
            {
                await _next(context);
            }
        }

        private bool CheckAuditLogPath(string path)
        {
            if(MixCmsHelper.CheckStaticFileRequest(path))
            {
                return false;
            }

            // Not log Signalr Hub 
            if (path.IndexOf("/hub") == 0)
            {
                return false;
            }
            return true;
        }

        private async Task LogRequest(HttpContext context)
        {
            var idService = context.RequestServices.GetService(typeof(MixIdentityService)) as MixIdentityService;
            var request = await FormatRequest(context.Request);
            _auditlogData.CreatedBy = idService.GetClaim(context.User, MixClaims.Username);
            _auditlogData.RequestIp = context.Request.HttpContext?.Connection?.RemoteIpAddress?.ToString();
            _auditlogData.Endpoint = context.Request.Path;
            _auditlogData.Method = context.Request.Method;
            _auditlogData.QueryString = context.Request.QueryString.ToString();
            _auditlogData.Body = request.IsJsonString() ? JObject.Parse(request) : new JObject(new JProperty("data", request));
        }
        private async Task LogResponse(HttpContext context)
        {
            if (context.Response.Body.CanSeek)
            {
                var response = await FormatResponse(context.Response);
                _auditlogData.StatusCode = context.Response.StatusCode;
                _auditlogData.Response = response.IsJsonString() ? JObject.Parse(response) : new JObject(new JProperty("data", response));
            }
        }

        private async Task<string> FormatResponse(HttpResponse response)
        {
            //We need to read the response stream from the beginning...
            response.Body.Seek(0, SeekOrigin.Begin);

            //...and copy it into a string
            string text = await new StreamReader(response.Body).ReadToEndAsync();

            //We need to reset the reader for the response so that the client can read it.
            response.Body.Seek(0, SeekOrigin.Begin);

            //Return the string for the response, including the status code (e.g. 200, 404, 401, etc.)
            return text;
        }

        private async Task<string> FormatRequest(HttpRequest request)
        {
            if (request.ContentLength > 0)
            {
                //This line allows us to set the reader for the request back at the beginning of its stream.
                request.EnableBuffering();

                //We now need to read the request stream.  First, we create a new byte[] with the same length as the request stream...
                var buffer = new byte[Convert.ToInt32(request.ContentLength)];

                //...Then we copy the entire request stream into the new buffer.
                await request.Body.ReadAsync(buffer, 0, buffer.Length);

                //We convert the byte[] into a string using UTF8 encoding...
                var bodyAsText = Encoding.UTF8.GetString(buffer);

                //..and finally, assign the read body back to the request body, which is allowed because of EnableRewind()
                request.Body.Seek(0, SeekOrigin.Begin);
                return bodyAsText;
            }
            return string.Empty;
        }
    }
}
