namespace Mix.Auth.Models
{
    public class LoginResponseModel
    {
        public string AESKey { get; set; }
        public string RSAKey { get; set; }
        public string Message { get; set; }
    }
}
