using Microsoft.Extensions.DependencyInjection.Extensions;
using Mix.Mq;
using Mix.Mq.Lib.Models;
using Mix.Mq.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.TryAddSingleton<MixQueueMessages<MessageQueueModel>>();

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddCors(o => o.AddPolicy("AllowAll", builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader()
           .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding");
}));

var app = builder.Build();

app.MapDefaultEndpoints();
// Configure the HTTP request pipeline.
//app.MapGrpcService<MixMqService>();

app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });
app.UseRouting();
app.UseCors();
app.UseEndpoints(endpoints =>
{
endpoints.MapGrpcService<MixMqService>().EnableGrpcWeb()
.RequireCors("AllowAll"); ;
});

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
