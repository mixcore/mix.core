namespace Mix.Identity.Domain.Models
{
    public class LoginSuccessModel
    {
        public string AESKey { get; set; }
        public string RSAKey { get; set; }
        public string Message { get; set; }
    }
}
