using Microsoft.AspNetCore.Http;
using System.Text;

namespace Mix.Lib.Models
{
    public class ParsedRequestModel
    {
        public string RequestId { get; set; }
        public string Body { get; set; }
        public string RequestIp { get; set; }
        public string Endpoint { get; set; }
        public string Method { get; set; }

        public ParsedRequestModel()
        {

        }
        public ParsedRequestModel(HttpRequest request)
        {
            RequestId = request.Headers["RequestId"];
            Body = GetBody(request);
            RequestIp = $"{request.Headers.Referer} - {request.HttpContext.Connection.RemoteIpAddress}";
            Endpoint = request.Path;
            Method = request.Method;
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
