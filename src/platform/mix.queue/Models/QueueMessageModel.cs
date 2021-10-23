using Newtonsoft.Json.Linq;

namespace Mix.Queue.Models
{
    public class QueueMessageModel
    {
        public string Action { get; set; }
        public string Status { get; set; }
        public string FullName { get; set; }
        public JObject Model { get; set; }

        public void Package<T>(T data)
        {
            FullName = typeof(T).FullName;
            Model = JObject.FromObject(data);
        }
    }
}
