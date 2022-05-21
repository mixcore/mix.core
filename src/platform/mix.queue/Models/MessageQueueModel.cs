using Mix.Heart.Helpers;
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
        public MessageQueueModel(string topicId, string action, object data = null)
        {
            TopicId = topicId;
            Action = action;
            if (data != null)
            {
                Data = ReflectionHelper.ParseObject(data);
            }
        }

        public void Package<T>(T data)
        {
            TopicId = typeof(T).FullName;
            Data = ReflectionHelper.ParseObject(data);
        }
    }
}
