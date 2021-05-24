using Newtonsoft.Json;

namespace Mix.Cms.Lib.Dtos
{
    public class RenewTokenDto
    {
        [JsonProperty("refreshToken")]
        public string RefreshToken { get; set; }
        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }
    }
}
