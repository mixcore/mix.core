using Mix.Database.Entities.Account;
using System.Configuration;
using System.Reflection;
using Mix.Log.Lib;
using Microsoft.Azure.Amqp.Framing;
using Mix.Lib.Middlewares;
using Mix.Log.Lib.Publishers;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Mix.Log.Lib.Interfaces;
using Mix.Log.Lib.Services;

var builder = MixCmsHelper.CreateWebApplicationBuilder(args);
if (builder.Environment.IsDevelopment())
{
    builder.AddServiceDefaults();
}
if (!Directory.Exists(MixFolders.MixContentSharedFolder))
{
    MixFileHelper.CopyFolder(MixFolders.MixCoreConfigurationFolder, MixFolders.MixContentSharedFolder);
}


builder.Services.AddMixServices(Assembly.GetExecutingAssembly(), builder.Configuration);
builder.Services.AddMixCors();
builder.AddMixLogPublisher();

// Must app Auth config after Add mixservice to init App config 
builder.Services.AddMixAuthorize<MixCmsAccountContext>(builder.Configuration);

var app = builder.Build();

app.UseMixCors();
app.UseMixTenant();
app.UseRouting();
app.UseMixAuth();
// auditlog middleware must go after auth
app.UseMiddleware<AuditlogMiddleware>();
app.UseMixCors();
app.UseRouting();
app.UseMixApps(Assembly.GetExecutingAssembly(), builder.Configuration, builder.Environment.ContentRootPath, builder.Environment.IsDevelopment());

app.Run();
