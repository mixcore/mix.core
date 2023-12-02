using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

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
            var credentials = CallCredentials.FromInterceptor((context, metadata) =>
            {
                if (!string.IsNullOrEmpty(_token))
                {
                    metadata.Add("Authorization", _token);
                }
                return Task.CompletedTask;
            });

            // SslCredentials is used here because this channel is using TLS.
            // CallCredentials can't be used with ChannelCredentials.Insecure on non-TLS channels.
            var channel = GrpcChannel.ForAddress(address, new GrpcChannelOptions
            {
                Credentials = ChannelCredentials.Create(new SslCredentials(), credentials)
            });
            return channel;
        }
    }
}
