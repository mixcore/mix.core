namespace Mix.Queue.Models.QueueSetting
{
    public class AzureQueueSetting : QueueSetting
    {
        public string ConnectionString { get; set; }
        public string QueueName { get; set; }
    }
}
