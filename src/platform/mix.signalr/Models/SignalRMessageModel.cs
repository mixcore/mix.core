using Mix.Heart.Helpers;
using Mix.SignalR.Enums;
using Newtonsoft.Json.Linq;
using System;

namespace Mix.SignalR.Models
{
    public class SignalRMessageModel
    {
        public SignalRMessageModel()
        {
        }
        public SignalRMessageModel(object data)
        {
            Data = ReflectionHelper.ParseObject(data);
        }

        public string Title { get; set; }
        public string Message { get; set; }
        public HubMessageType Type { get; set; } = HubMessageType.Info;
        public JObject Data { get; set; }
        public DateTime CreatedDateTime => DateTime.UtcNow;
        public override string ToString()
        {
            return ReflectionHelper.ParseObject(this).ToString();
        }
    }
}
