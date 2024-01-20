namespace Mix.Queue.Models.QueueSetting
{
    public class AzureQueueSetting : IQueueSetting
    {
        public string ConnectionString { get; set; }
        public string QueueName { get; set; }
    }
}
