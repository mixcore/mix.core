namespace Mix.Queue.Models.QueueSetting
{
    public class GoogleQueueSetting : IQueueSetting
    {
        public string CredentialFile { get; set; }
        public string ProjectId { get; set; }
    }
}
