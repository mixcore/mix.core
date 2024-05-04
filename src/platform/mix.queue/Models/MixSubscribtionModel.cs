namespace Mix.Mq.Lib.Models
{
    public class MixSubscriptionModel
    {
        public string TopicId { get; set; }
        public string Id { get; set; }
        public MixQueueMessageLogState Status { get; set; }
    }
}
