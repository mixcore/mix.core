namespace Mix.Shared.Models
{
    public class SmtpConfiguration
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public bool SSL { get; set; }
        public string User { get; set; }
    }
}
