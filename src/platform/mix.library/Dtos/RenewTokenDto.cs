using Newtonsoft.Json;
using System;

namespace Mix.Lib.Dtos
{
    public class RenewTokenDto
    {
        public Guid RefreshToken { get; set; }
        public string AccessToken { get; set; }
    }
}
