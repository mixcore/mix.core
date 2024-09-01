using Microsoft.Extensions.DependencyInjection.Extensions;
using Mix.Constant.Constants;
using Mix.Heart.Services;
using Mix.Lib.Helpers;
using Mix.Log.Lib;
using Mix.Mq.Lib.Models;
using Mix.Mq.Server.Domain.Services;
using OpenTelemetry;
using System.Configuration;
using System.Reflection;
using Mix.Queue.Extensions;
using Mix.Database.Entities.Account;
using Mix.Service.Interfaces;
using Mix.Service.Services;
using Mix.Database.Entities.Cms;
using Mix.Database.Services;
using Mix.Lib.Interfaces;
using Mix.Queue.Interfaces;
using Mix.Queue.Services;
using Mix.Shared.Services;
using Mix.SignalR.Interfaces;
using Mix.Lib.Services;

if (Directory.Exists($"../{MixFolders.MixCoreConfigurationFolder}"))
{
    MixFileHelper.CopyFolder($"../{MixFolders.MixCoreConfigurationFolder}", MixFolders.MixContentSharedFolder);
}


var builder = MixCmsHelper.CreateWebApplicationBuilder(args);
if (builder.Environment.IsDevelopment())
{
    builder.AddServiceDefaults();
}

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.Configure<HostOptions>(options =>
{
    options.ServicesStartConcurrently = true;
    options.ServicesStopConcurrently = false;
});
builder.AddMixQueue();
builder.Services.TryAddSingleton<GrpcStreamingService>();
builder.Services.TryAddSingleton<MixQueueMessages<MessageQueueModel>>();
builder.Services.AddHostedService<MixMqSubscriptionService>();

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddMixCors();
builder.Services.AddCors(o => o.AddPolicy("AllowAllGrpc", builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader()
           .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding");
}));

builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.TryAddSingleton<MixEndpointService>();
builder.Services.TryAddSingleton<DatabaseService>();
builder.Services.TryAddSingleton<IMemoryQueueService<MessageQueueModel>, MemoryQueueService>();
builder.Services.TryAddSingleton<ILogStreamHubClientService, LogStreamHubClientService>();
builder.Services.TryAddSingleton<IPortalHubClientService, PortalHubClientService>();
builder.Services.TryAddSingleton<IMixTenantService, MixTenantService>();

builder.Services.AddMixSignalR(builder.Configuration);
builder.Services.AddMixCommunicators();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapDefaultEndpoints();
}

// Configure the HTTP request pipeline.

app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });
app.UseRouting();
app.UseMixCors();
app.UseEndpoints(endpoints =>
{
    endpoints.MapGrpcService<MixMqService>().EnableGrpcWeb()
    .RequireCors("AllowAllGrpc");
    endpoints.UseMixSignalRApp();
});
app.UseSwagger();
app.UseSwaggerUI();
app.UseMixSwaggerApps(app.Environment.IsDevelopment(), Assembly.GetExecutingAssembly());
app.MapControllers();
app.Run();