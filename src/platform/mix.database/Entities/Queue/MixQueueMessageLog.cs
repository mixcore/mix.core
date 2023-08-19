using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Database.Entities.Queue
{
    public sealed class MixQueueMessageLog: EntityBase<Guid>
    {
        public Guid QueueMessageId { get; set; }
        public string TopicId { get; set; }
        public string SubscriptionId { get; set; }
        public string Action { get; set; }
        public string StringData { get; set; }
        public JObject ObjectData { get; set; }
        public string DataTypeFullName { get; set; }
        public string Note { get; set; }
        public MixQueueMessageLogState State { get; set; }
        public int TenantId { get; set; }
    }
}
