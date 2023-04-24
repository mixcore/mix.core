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
            Data = ReflectionHelper.ParseObject(data).ToString(Newtonsoft.Json.Formatting.None);
        }
        public HubUserModel From { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public MessageAction Action { get; set; } = MessageAction.NewMessage;
        public MessageType Type { get; set; } = MessageType.Info;
        public string Data { get; set; }
        public DateTime CreatedDateTime => DateTime.UtcNow;
        public override string ToString()
        {
            return ReflectionHelper.ParseObject(this).ToString();
        }
    }
}
