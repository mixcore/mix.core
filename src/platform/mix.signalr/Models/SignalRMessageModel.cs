using Mix.Heart.Helpers;
using Mix.SignalR.Enums;
using Newtonsoft.Json.Linq;

namespace Mix.SignalR.Models
{
    public class SignalRMessageModel<T>
    {
        public HubMessageType Type { get; set; }
        public T Message { get; set; }

        public override string ToString()
        {
            return ReflectionHelper.ParseObject(this).ToString();
        }
    }
}
