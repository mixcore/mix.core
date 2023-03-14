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
        public string Data { get; set; }
        public string DataTypeFullName { get; set; }

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
                try
                {
                    DataTypeFullName = data.GetType().FullName;
                    Data = ReflectionHelper.ParseObject(data).ToString(Newtonsoft.Json.Formatting.None);
                }
                catch
                {
                    Data = data.ToString();
                }
            }
        }

        public void Package<T>(T data)
        {
            TopicId = typeof(T).FullName;
            Data = ReflectionHelper.ParseObject(data).ToString(Newtonsoft.Json.Formatting.None);
        }

        public T ParseData<T>()
        {
            try
            {
                return JObject.Parse(Data).ToObject<T>();
            }
            catch
            {
                return default(T);
            }
        }
    }
}
