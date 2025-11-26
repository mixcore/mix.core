using Newtonsoft.Json.Linq;

namespace Mix.Shared.Models
{
    public class HttpRequestModel
    {
        public string Method { get; set; }
        public string RequestUrl { get; set; }
        public JObject? Body { get; set; }
        public Dictionary<string, string>? QueryParams { get; set; }
        public string? BearerToken { get; set; }
        public List<KeyValuePair<string, string>>? Headers { get; set; }
    }
}
