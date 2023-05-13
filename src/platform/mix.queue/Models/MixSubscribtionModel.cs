

namespace Mix.Queue.Models
{
    public class MixSubscriptionModel
    {
        public string TopicId { get; set; }
        public string Id { get; set; }
        public MixQueueMessageStatus Status { get; set; }
    }
}
