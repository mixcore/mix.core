using Mix.Common.Domain.Enums;
using System.Text;

namespace Mix.Common.Domain.Dtos
{
    public class CryptoMessageDto
    {
        public string Key { get; set; }
        public JObject ObjectData { get; set; }
        public string StringData { get; set; }
        public MixEncoding EncodingType { get; set; }
        public Encoding GetEncoding()
        {
            return EncodingType switch
            {
               MixEncoding.UNICODE  => Encoding.Unicode,
               _ => Encoding.UTF8
            };
        }
    }
}
