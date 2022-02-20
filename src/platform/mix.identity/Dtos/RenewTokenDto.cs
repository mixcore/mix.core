using System;

namespace Mix.Identity.Dtos
{
    public class RenewTokenDto
    {
        public Guid RefreshToken { get; set; }
        public string AccessToken { get; set; }
    }
}
