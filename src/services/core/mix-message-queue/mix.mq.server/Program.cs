using Microsoft.Extensions.DependencyInjection.Extensions;
using Mix.Constant.Constants;
using Mix.Database.Entities.Account;
using Mix.Database.Entities.Cms;
using Mix.Heart.Services;
using Mix.Lib.Helpers;
using Mix.Log.Lib;
using Mix.Mq.Lib.Models;
using Mix.Mq.Server.Domain.Services;
using Mix.Queue.Extensions;
using Mix.Service.Interfaces;
using Mix.Service.Services;
using System.Reflection;

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

builder.Services.AddMixLog(builder.Configuration);
builder.Services.AddMixSignalR(builder.Configuration);
builder.Services.AddMixCommunicators();
builder.Services.AddSwaggerGen();
builder.Services.TryAddSingleton<IMixMemoryCacheService, MixMemoryCacheService>();
builder.Services.AddDbContext<MixCmsAccountContext>();
builder.Services.AddDbContext<MixCmsContext>();
builder.Services.AddMixIdentityConfigurations<MixCmsAccountContext>(builder.Configuration);
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapDefaultEndpoints();
}

// Configure the HTTP request pipeline.

app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });
app.UseRouting();
app.UseMixCors();
app.UseMixAuth();
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