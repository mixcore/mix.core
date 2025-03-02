using Mix.Database.Entities.Account;
using System.Reflection;
using Mix.Log.Lib;
using Mix.Lib.Middlewares;
using Mix.Lib.Services;
using Mix.Mq.Lib.Models;
using Mix.Queue.Interfaces;
using Mix.Queue.Services;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Mix.Lib.Publishers;
using Mix.Lib.Subscribers;
using Mix.Shared.Extensions;

var builder = MixCmsHelper.CreateWebApplicationBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.AddConfigurations();
builder.AddMixServices(Assembly.GetExecutingAssembly());
builder.AddMixCors();
// Must app Auth config after Add mixservice to init App config 
builder.Services.AddMixAuthorize<MixCmsAccountContext>(builder.Configuration);
builder.Services.AddScoped<MixIdentityService>();
builder.Services.TryAddSingleton<IMemoryQueueService<MessageQueueModel>, MemoryQueueService>();
builder.Services.AddHostedService<MixBackgroundTaskPublisher>();
builder.Services.AddHostedService<MixBackgroundTaskSubscriber>();
builder.AddMixLogPublisher();
var app = builder.Build();
app.MapDefaultEndpoints();
app.UseMixTenant();
app.UseMixCors(builder.Configuration);
app.UseRouting();
app.UseMixAuth();
// auditlog middleware must go after auth
app.UseMiddleware<AuditlogMiddleware>();
app.UseMixApps(Assembly.GetExecutingAssembly(), builder.Configuration, !builder.Environment.IsProduction());

app.Run();
