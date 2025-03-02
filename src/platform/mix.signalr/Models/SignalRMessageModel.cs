using Mix.Database.Entities.MixDb;
using Mix.Heart.Extensions;
using Mix.Heart.Helpers;
using Mix.SignalR.Enums;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Reflection.Metadata;
using System.Text.Json;

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

        public int? DeviceId { get; set; }
        public MessageAction Action { get; set; } = MessageAction.NewMessage;
        public MessageType Type { get; set; } = MessageType.Info;
        public object Data { get; set; }
        public DateTime CreatedDateTime => DateTime.UtcNow;
        public override string ToString()
        {
            if (Data != null && Data.GetType() == typeof(JsonElement))
            {
                var strData = Data.ToString();
                if (strData.IsJsonString())
                {
                    Data = JObject.Parse(strData);
                }
                else if (strData.IsJArrayString())
                {
                    Data = JArray.Parse(strData);
                }
            };

            return ReflectionHelper.ParseObject(this).ToString(Newtonsoft.Json.Formatting.None);
        }
    }
}
