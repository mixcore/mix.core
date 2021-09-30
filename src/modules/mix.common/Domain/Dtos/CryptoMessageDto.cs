using Newtonsoft.Json.Linq;

namespace Mix.Common.Domain.Dtos
{
    public class CryptoMessageDto
    {
        public string Key { get; set; }
        public JObject ObjectData { get; set; }
        public string StringData { get; set; }
    }
}
