using Microsoft.AspNetCore.Http;
using Mix.Service.Services;
using Mix.Shared.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Mix.Service.Models
{
    public class AuditLogDataModel
    {
        public string RequestIp { get; set; }
        public string Endpoint { get; set; }
        public string Method { get; set; }
        public string QueryString { get; set; }
        public JObject Body { get; set; }
        public JObject Response { get; set; }
        public JObject Exception { get; set; }
        public string CreatedBy { get; set; }

        public AuditLogDataModel()
        {
        }

        public void InitRequest(string createdBy, HttpContext context)
        {
            CreatedBy = createdBy;
            RequestIp = context.Request.HttpContext?.Connection?.RemoteIpAddress?.ToString();
            Endpoint = context.Request.Path;
            Method = context.Request.Method;
            Body = GetBodyAsync(context.Request);
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
