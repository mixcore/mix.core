using Newtonsoft.Json.Linq;

namespace Mix.Shared.Models
{
    public class HttpRequestModel
    {
        public string Method { get; set; }
        public string RequestUrl { get; set; }
        public JObject Body { get; set; }
    }
}
