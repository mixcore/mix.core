using Newtonsoft.Json.Linq;

namespace Mix.Database.Entities.QueueLog
{
    public sealed class QueueLog : EntityBase<Guid>
    {
        public Guid? QueueMessageId { get; set; }
        public string TopicId { get; set; }
        public string SubscriptionId { get; set; }
        public string Action { get; set; }
        public string StringData { get; set; }
        public JObject ObjectData { get; set; }
        public JObject Exception { get; set; }
        public JArray Subscriptions { get; set; }
        public string DataTypeFullName { get; set; }
        public string Note { get; set; }
        public MixQueueMessageLogState State { get; set; }
        public int TenantId { get; set; }
    }
}
