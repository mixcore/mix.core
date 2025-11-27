// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Mix.Mqtt.Lib.Helpers;
using MQTTnet.Server;

namespace MQTTnet.Samples.Server;

public static class Server_TLS_Samples
{
    public static async Task Run_Server_With_Self_Signed_Certificate()
    {
        /*
         * This sample starts a simple MQTT server which will accept any TCP connection.
         * It also has an encrypted connection using a self signed TLS certificate.
         *
         * See sample "Run_Minimal_Server" for more details.
         */

        var mqttServerFactory = new MqttServerFactory();

        // This certificate is self signed so that
        var certificate = CertificateHelper.CreateSelfSignedCertificate("localhost", "1.3.6.1.5.5.7.3.1");

        var mqttServerOptions = new MqttServerOptionsBuilder().WithEncryptionCertificate(certificate).WithEncryptedEndpoint().Build();

        using (var mqttServer = mqttServerFactory.CreateMqttServer(mqttServerOptions))
        {
            await mqttServer.StartAsync();

            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();

            // Stop and dispose the MQTT server if it is no longer needed!
            await mqttServer.StopAsync();
        }
    }

}