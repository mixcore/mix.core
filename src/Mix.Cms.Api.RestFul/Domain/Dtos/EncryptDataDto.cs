using Newtonsoft.Json.Linq;

namespace Mix.Cms.Api.RestFul.Domain.Dtos
{
    public class EncryptDataDto
    {
        public string Key { get; set; }
        public JObject ObjectData { get; set; }
        public string StringData { get; set; }
    }
}
