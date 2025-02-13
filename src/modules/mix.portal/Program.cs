using Mix.Database.Entities.Account;
using System.Reflection;
using Mix.Log.Lib;
using Mix.Lib.Middlewares;
using Mix.Shared.Extensions;

var builder = MixCmsHelper.CreateWebApplicationBuilder(args);
builder.AddServiceDefaults();

builder.AddConfigurations();
builder.AddMixServices(Assembly.GetExecutingAssembly());
builder.AddMixCors();
builder.AddMixLogPublisher();

// Must app Auth config after Add mixservice to init App config 
builder.Services.AddMixAuthorize<MixCmsAccountContext>(builder.Configuration);

var app = builder.Build();
app.MapDefaultEndpoints();
app.UseMixCors(builder.Configuration);
app.UseMixTenant();
app.UseRouting();
app.UseMixAuth();
// auditlog middleware must go after auth
app.UseMiddleware<AuditlogMiddleware>();
app.UseRouting();
app.UseMixApps(Assembly.GetExecutingAssembly(), builder.Configuration, builder.Environment.IsLocal());

app.Run();
