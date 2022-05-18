using Mix.Heart.Helpers;
using Mix.SignalR.Enums;
using System;

namespace Mix.SignalR.Models
{
    public class SignalRMessageModel<T>
    {
        public SignalRMessageModel()
        {

        }
        public SignalRMessageModel(T message)
        {
            Message = message;
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public HubMessageType Type { get; set; }
        public T Message { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public override string ToString()
        {
            CreatedDateTime = DateTime.UtcNow;
            return ReflectionHelper.ParseObject(this).ToString();
        }
    }
}
