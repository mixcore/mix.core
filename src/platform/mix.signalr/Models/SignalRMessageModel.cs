using Mix.Heart.Helpers;
using Mix.SignalR.Enums;
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
            Data = data;
        }
        public HubUserModel From { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public MessageAction Action { get; set; } = MessageAction.NewMessage;
        public MessageType Type { get; set; } = MessageType.Info;
        public dynamic Data { get; set; }
        public DateTime CreatedDateTime => DateTime.UtcNow;
        public override string ToString()
        {
            return ReflectionHelper.ParseObject(this).ToString();
        }
    }
}
