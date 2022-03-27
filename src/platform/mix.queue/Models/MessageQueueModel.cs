using Mix.Shared.Enums;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Mix.Queue.Models
{
    public class MessageQueueModel
    {
        public string Action { get; set; }
        public bool Success { get; set; }
        public string TopicId { get; set; }
        public JObject Model { get; set; }

        public List<MixSubscribtionModel> Subscriptions { get; set; } = new();

        public void Package<T>(T data)
        {
            TopicId = typeof(T).FullName;
            Model = JObject.FromObject(data);
        }
    }
}
