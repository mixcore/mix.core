using System;

namespace Mix.Auth.Dtos
{
    public class RenewTokenDto
    {
        public Guid RefreshToken { get; set; }
        public string AccessToken { get; set; }
    }
}
