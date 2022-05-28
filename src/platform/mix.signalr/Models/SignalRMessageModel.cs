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
        public SignalRMessageModel(T data)
        {
            Data = data;
        }

        public string Title { get; set; }
        public string Message { get; set; }
        public HubMessageType Type { get; set; } = HubMessageType.Info;
        public T Data { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public override string ToString()
        {
            CreatedDateTime = DateTime.UtcNow;
            return ReflectionHelper.ParseObject(this).ToString();
        }
    }
}
