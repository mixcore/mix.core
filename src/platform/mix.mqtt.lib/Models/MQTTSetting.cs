using MQTTnet.Protocol;
using System.Security.Authentication;

namespace Mix.Mqtt.Lib.Models
{
    public class MQTTSetting
    {
        public string HostName { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? ClientId { get; set; }
        public string? Topic { get; set; }
        public int Port { get; set; }
        public bool UseTls { get; set; }
        public SslProtocols SslProtocol { get; set; } = SslProtocols.Tls12;
        public bool UseWebSocket { get; set; }
        public MqttQualityOfServiceLevel Qos { get; set; } = MqttQualityOfServiceLevel.AtLeastOnce;
    }
}
