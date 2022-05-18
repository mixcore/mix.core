using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Mix.Queue.Models
{
    public class MessageQueueModel
    {
        public string Action { get; set; }
        public bool Success { get; set; }
        public string TopicId { get; set; }
        public JObject Data { get; set; }

        public List<MixSubscribtionModel> Subscriptions { get; set; } = new();

        public MessageQueueModel()
        {

        }
        public MessageQueueModel(string action, string topicId, object data)
        {
            Action = action;
            TopicId = topicId;
            Data = JObject.FromObject(data);
        }

        public void Package<T>(T data)
        {
            TopicId = typeof(T).FullName;
            Data = JObject.FromObject(data);
        }
    }
}
