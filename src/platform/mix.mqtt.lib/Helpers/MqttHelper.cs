using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Mix.Mqtt.Lib.Models;
using MQTTnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Mqtt.Lib.Helpers
{
    public static class MqttHelper
    {
        public static MqttClientOptions? GetClientOptions(MQTTSetting? queueSetting)
        {
            queueSetting ??= new MQTTSetting();
            if (!string.IsNullOrEmpty(queueSetting.HostName))
            {

                MqttClientOptionsBuilder builder = new MqttClientOptionsBuilder();
                if (queueSetting.UseTls)
                {
                    builder.WithTlsOptions(new MqttClientTlsOptions()
                    {
                        UseTls = true,
                        SslProtocol = queueSetting.SslProtocol,
                        CertificateValidationHandler = (certificate) => true
                    });
                }
                if (!string.IsNullOrEmpty(queueSetting.UserName))
                {
                    builder.WithCredentials(queueSetting.UserName, queueSetting.Password);
                }
                if (queueSetting.UseWebSocket)
                {
                    builder.WithWebSocketServer(config =>
                    {
                        config.WithUri(queueSetting.HostName);
                    });
                }
                else
                {
                    builder.WithTcpServer(queueSetting.HostName, queueSetting.Port);
                }
                return builder.WithWillQualityOfServiceLevel(queueSetting.Qos).Build();
            }
            return default;
        }

        #region Helpers
        #endregion
        public static string GetTopicId(string topicName, string? clientId = null)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                return topicName;
            }
            return $"{topicName}/{clientId}";
        }
        public static string? GetClientId(string? clientId, string? userName)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                return userName;
            }
            return clientId;
        }
    }
}
