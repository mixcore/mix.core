using Newtonsoft.Json.Linq;

namespace Mix.Services.Graphql.Lib.Models
{
    public class QueryRequest
    {
        public string OperationName { get; set; }
        public string NamedQuery { get; set; }
        public string Query { get; set; }
        public JObject Variables { get; set; }
    }

}
