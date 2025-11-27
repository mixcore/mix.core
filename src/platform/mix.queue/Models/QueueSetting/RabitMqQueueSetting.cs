namespace Mix.Queue.Models.QueueSetting
{
    public class RabitMqQueueSetting : IQueueSetting
    {
        public string HostName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int? Port { get; set; }
        public string VHost { get; set; }
    }
}
