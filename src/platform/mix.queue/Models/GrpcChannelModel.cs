﻿using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Microsoft.AspNetCore.Http;
using System;
using System.Net.Http;

namespace Mix.Queue.Models
{
    public class GrpcChannelModel<T>
    {
        public readonly T Client;
        private GrpcChannel _channel;
        private string _token;

        public GrpcChannelModel(string address, HttpContext context = null)
        {
            _token = context?.Request.Headers["Authorization"].ToString();
            _channel = CreateChannel(address);
            var ctor = typeof(T).GetConstructor(new Type[] { typeof(GrpcChannel) });
            if (ctor != null)
            {
                Client = (T)ctor.Invoke(new object[] { _channel });
            }
        }

        private GrpcChannel CreateChannel(string address)
        {
            //var credentials = CallCredentials.FromInterceptor((context, metadata) =>
            //{
            //    if (!string.IsNullOrEmpty(_token))
            //    {
            //        metadata.Add("Authorization", _token);
            //    }
            //    return Task.CompletedTask;
            //});

            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            // This switch must be set before creating the GrpcChannel/HttpClient.
            AppContext.SetSwitch(
                "System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            // SslCredentials is used here because this channel is using TLS.
            // CallCredentials can't be used with ChannelCredentials.Insecure on non-TLS channels.
            var channel = GrpcChannel.ForAddress(address, new GrpcChannelOptions
            {
                HttpHandler = new GrpcWebHandler(handler)
            });
            return channel;
        }
    }
}
