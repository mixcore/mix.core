using MQTTnet.Protocol;
using System.Security.Authentication;

namespace Mix.Mqtt.Lib.Models
{
    public class MQTTSetting
    {
        public MQTTSetting()
        {
                
        }
        public MQTTSetting(string webSocketUrl)
        {
            var uri = new Uri(webSocketUrl);
            WebSocketUrl = webSocketUrl;
            HostName = uri.Host;
            Port = uri.Port;
            UseTls = uri.Scheme == "wss";
        }

        public string? WebSocketUrl { get; set; }
        public string? HostName { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? ClientId { get; set; }
        public string? Topic { get; set; }
        public int Port { get; set; } = 443;
        public bool UseTls { get; set; }
        public SslProtocols SslProtocol { get; set; } = SslProtocols.Tls12;
        public bool UseWebSocket { get; set; } = true;
        public MqttQualityOfServiceLevel Qos { get; set; } = MqttQualityOfServiceLevel.AtLeastOnce;
        public string? V { get; }
    }
}
