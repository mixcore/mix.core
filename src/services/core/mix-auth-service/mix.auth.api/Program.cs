using Mix.Constant.Constants;
using Mix.Database.Entities.Account;
using Mix.Heart.Services;
using System.Configuration;
using System.Reflection;
using Mix.Log.Lib;
using Microsoft.Azure.Amqp.Framing;
using Mix.Lib.Middlewares;
using Mix.Lib.Services;
using Mix.Mq.Lib.Models;
using Mix.Queue.Interfaces;
using Mix.Queue.Services;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Mix.Lib.Publishers;
using Mix.Lib.Subscribers;
using Mix.Log.Lib.Subscribers;
using Mix.Log.Lib.Publishers;
using Mix.Log.Lib.Interfaces;
using Mix.Log.Lib.Services;

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

builder.Services.AddMixServices(Assembly.GetExecutingAssembly(), builder.Configuration);
builder.Services.AddMixCors();
// Must app Auth config after Add mixservice to init App config 
builder.Services.AddMixAuthorize<MixCmsAccountContext>(builder.Configuration);
builder.Services.AddScoped<MixIdentityService>();
builder.Services.TryAddSingleton<IMemoryQueueService<MessageQueueModel>, MemoryQueueService>();
builder.Services.AddHostedService<MixBackgroundTaskPublisher>();
builder.Services.AddHostedService<MixBackgroundTaskSubscriber>();
builder.AddMixLogPublisher();
var app = builder.Build();

app.UseMixTenant();
app.UseMixCors();
app.UseRouting();
app.UseMixAuth();
// auditlog middleware must go after auth
app.UseMiddleware<AuditlogMiddleware>();
app.UseMixCors();
app.UseMixApps(Assembly.GetExecutingAssembly(), builder.Configuration, builder.Environment.ContentRootPath, builder.Environment.IsDevelopment());

app.Run();
