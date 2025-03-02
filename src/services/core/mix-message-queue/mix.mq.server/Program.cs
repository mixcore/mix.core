using Microsoft.Extensions.DependencyInjection.Extensions;
using Mix.Constant.Constants;
using Mix.Heart.Services;
using Mix.Lib.Helpers;
using Mix.Mq.Lib.Models;
using Mix.Mq.Server.Domain.Services;
using System.Reflection;
using Mix.Queue.Extensions;
using Mix.Database.Entities.Account;
using Mix.Service.Interfaces;
using Mix.Service.Services;
using Mix.Database.Entities.Cms;
using Mix.Lib.Interfaces;
using Mix.Queue.Interfaces;
using Mix.Queue.Services;
using Mix.SignalR.Interfaces;
using Mix.Lib.Services;
using Mix.Shared.Extensions;
using Mix.Database.Services.MixGlobalSettings;


var builder = MixCmsHelper.CreateWebApplicationBuilder(args);
builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.Configure<HostOptions>(options =>
{
    options.ServicesStartConcurrently = true;
    options.ServicesStopConcurrently = false;
});
builder.AddConfigurations();
builder.AddMixQueue();
builder.Services.TryAddSingleton<GrpcStreamingService>();
builder.Services.TryAddSingleton<MixQueueMessages<MessageQueueModel>>();
builder.Services.AddHostedService<MixMqSubscriptionService>();

// Add services to the container.
builder.Services.AddGrpc();
builder.AddMixCors();
builder.Services.AddCors(o => o.AddPolicy("AllowAllGrpc", builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader()
           .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding");
}));


builder.Services.TryAddSingleton<DatabaseService>();
builder.Services.TryAddSingleton<IMemoryQueueService<MessageQueueModel>, MemoryQueueService>();
builder.Services.TryAddSingleton<ILogStreamHubClientService, LogStreamHubClientService>();
builder.Services.TryAddSingleton<IPortalHubClientService, PortalHubClientService>();
builder.Services.TryAddSingleton<IMixTenantService, MixTenantService>();

string? azureConnectionString = builder.Configuration.GetSection("Azure")["SignalRConnectionString"];
string? redisConnectionString = builder.Configuration.GetSection("Redis")["ConnectionStrings"];
builder.AddMixSignalR(azureConnectionString, redisConnectionString);

builder.Services.AddMixCommunicators();
builder.Services.AddSwaggerGen();
builder.Services.TryAddSingleton<IMixMemoryCacheService, MixMemoryCacheService>();
builder.Services.AddDbContext<MixCmsAccountContext>();
builder.Services.AddDbContext<MixCmsContext>();
builder.Services.AddMixIdentityConfigurations<MixCmsAccountContext>(builder.Configuration);
var app = builder.Build();

app.MapDefaultEndpoints();
// Configure the HTTP request pipeline.

app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });
app.UseRouting();
app.UseMixCors(builder.Configuration);
app.UseMixAuth();
app.UseEndpoints(endpoints =>
{
    endpoints.MapGrpcService<MixMqService>().EnableGrpcWeb()
    .RequireCors("AllowAllGrpc");
    endpoints.UseMixSignalRApp();
});
app.UseSwagger();
app.UseSwaggerUI();
app.UseMixSwaggerApps(!app.Environment.IsProduction(), Assembly.GetExecutingAssembly());
app.MapControllers();
app.Run();