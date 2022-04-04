namespace Mix.Communicator.Models
{
    public class EmailSettingModel
    {
        public string Server { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string From { get; set; }
        public int Port { get; set; }
        public bool SSL { get; set; }

    }
}
