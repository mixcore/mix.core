using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Database.Entities.Queue
{
    public sealed class MixQueueMessage: EntityBase<Guid>
    {
        public string From { get; set; }
        public string Action { get; set; }
        public string TopicId { get; set; }
        public string StringData { get; set; }
        public JObject ObjectData { get; set; }
        public string DataTypeFullName { get; set; }
        public string State { get; set; }
        public int TenantId { get; set; }
    }
}
