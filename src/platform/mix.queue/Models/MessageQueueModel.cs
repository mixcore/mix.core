using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Mix.Queue.Models
{
    public class MessageQueueModel
    {
        public string Action { get; set; }
        public string Status { get; set; }
        public string FullName { get; set; }
        public JObject Model { get; set; }

        public List<MixSubscribtionModel> Subscriptions { get; set; } = new();

        public void Package<T>(T data)
        {
            FullName = typeof(T).FullName;
            Model = JObject.FromObject(data);
        }
    }
}
