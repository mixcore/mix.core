using Microsoft.AspNetCore.Http;
using Mix.Log.Lib.Services;
using Mix.Service.Services;
using Mix.Shared.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Mix.Log.Lib.Models
{
    public class AuditLogDataModel
    {
        public int StatusCode { get; set; }
        public string? RequestIp { get; set; }
        public string Endpoint { get; set; }
        public string Method { get; set; }
        public string QueryString { get; set; }
        public JObject? Body { get; set; }
        public JObject Response { get; set; }
        public JObject Exception { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        
        // HIPAA/GDPR Compliance Enhancement Fields
        public string CorrelationId { get; set; }
        public int? TenantId { get; set; }
        public bool PhiAccessFlag { get; set; }
        public string UserAgent { get; set; }
        public string SessionId { get; set; }

        public AuditLogDataModel()
        {
            CorrelationId = Guid.NewGuid().ToString();
            CreatedAt = DateTime.UtcNow;
        }

        public void InitRequest(string createdBy, HttpContext context)
        {
            CreatedBy = createdBy;
            RequestIp = context.Request.HttpContext?.Connection?.RemoteIpAddress?.ToString();
            Endpoint = context.Request.Path;
            Method = context.Request.Method;
            Body = GetBodyAsync(context.Request);
            
            // Set compliance enhancement fields
            UserAgent = context.Request.Headers["User-Agent"].FirstOrDefault();
            SessionId = context.Session?.Id;
            
            // Detect PHI access based on endpoint patterns
            PhiAccessFlag = DetectPhiAccess(context.Request.Path);
        }

        private bool DetectPhiAccess(string endpoint)
        {
            // Basic PHI detection based on endpoint patterns
            // This should be enhanced based on specific application endpoints
            var phiPatterns = new[] { "/health", "/medical", "/patient", "/diagnosis", "/treatment" };
            return phiPatterns.Any(pattern => endpoint.IndexOf(pattern, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private JObject? GetBodyAsync(HttpRequest request)
        {
            string? bodyStr = null;

            // Arguments: Stream, Encoding, detect encoding, buffer size 
            // AND, the most important: keep stream opened
            try
            {
                if (request.BodyReader != null && request.Method != "GET" && request.Method != "DELETE")
                {
                    request.EnableBuffering();
                    using (var reader = new StreamReader(request.BodyReader.AsStream(), Encoding.UTF8, true, 1024, true))
                    {
                        bodyStr = reader.ReadToEnd();
                    }
                    request.Body.Seek(0, SeekOrigin.Begin);
                    if (bodyStr.StartsWith("{") && bodyStr.EndsWith("}"))
                    {
                        return JObject.Parse(bodyStr);
                    }
                    else
                    {
                        return new JObject(new JProperty("data", bodyStr));
                    }
                }
            }
            catch
            {
                Console.WriteLine($"{nameof(AuditLogService)}: Cannot read body request");
            }
            return default;
        }

    }
}
