

namespace Mix.Queue.Models
{
    public class MixSubscribtionModel
    {
        public string TopicId { get; set; }
        public string Id { get; set; }
        public MixQueueMessageStatus Status { get; set; }
    }
}
