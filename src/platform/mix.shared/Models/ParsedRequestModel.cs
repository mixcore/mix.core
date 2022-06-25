namespace Mix.Shared.Models
{
    public class ParsedRequestModel
    {
        public string Body { get; set; }
        public string RequestIp { get; set; }
        public string Endpoint { get; set; }
        public string Method { get; set; }

        public ParsedRequestModel()
        {

        }
        public ParsedRequestModel(string ip, string endpoint, string method, string body)
        {
            RequestIp = ip;
            Endpoint = endpoint;
            Method = method;
            Body = body;
        }
    }
}
