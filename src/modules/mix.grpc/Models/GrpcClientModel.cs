using Grpc.Net.Client;
using System;

namespace Mix.Grpc.Models
{
    public class GrpcClientModel<T>
    {
        public readonly T Client;
        private GrpcChannel _channel;

        public GrpcClientModel(string address)
        {
            _channel = GrpcChannel.ForAddress(address);
            var ctor = typeof(T).GetConstructor(new Type[] { typeof(GrpcChannel) });
            if (ctor != null)
            {
                Client = (T)ctor.Invoke(new object[] { _channel });
            }
        }
    }
}
