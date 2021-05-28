using Newtonsoft.Json;

namespace Mix.Lib.Dtos
{
    public class RenewTokenDto
    {
        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }
    }
}
