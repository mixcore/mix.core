using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Mix.Mq.Lib.Models;
using Mix.Mq.Server.Domain.Services;
using Mix.Shared.Interfaces;

namespace Mix.Mq.Server.Domain
{
    public class StartupService : IStartupService
    {
        public void AddServices(IHostApplicationBuilder builder)
        {
            builder.Services.TryAddSingleton<GrpcStreamingService>();
            builder.Services.AddHostedService<MixMqSubscriptionService>();
            builder.Services.TryAddSingleton<MixQueueMessages<MessageQueueModel>>();
            builder.Services.AddGrpc();
            builder.Services.AddCors(o => o.AddPolicy("AllowAllGrpc", builder =>
            {
                builder.WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding");
            }));
        }

        public void UseApps(IApplicationBuilder app, IConfiguration configuration, bool isDevelop)
        {
            app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });
        }

        public void UseEndpoints(IEndpointRouteBuilder endpoints, IConfiguration configuration, bool isDevelop)
        {
            endpoints.MapGrpcService<MixMqService>().EnableGrpcWeb()
                .RequireCors("AllowAllGrpc");
        }
    }
}
