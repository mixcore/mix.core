using Mix.Database.Entities.Account;
using Mix.Lib.Middlewares;
using Mix.Log.Lib;
using System.Reflection;

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
builder.Services.AddMixLog(builder.Configuration);
// Must app Auth config after Add mixservice to init App config 
builder.Services.AddMixAuthorize<MixCmsAccountContext>(builder.Configuration);

var app = builder.Build();

app.UseMixCors();
app.UseMixTenant();
app.UseMiddleware<AuditlogMiddleware>();
app.UseRouting();
app.UseMixAuth();
app.UseMixCors();
app.UseRouting();
app.UseMixApps(Assembly.GetExecutingAssembly(), builder.Configuration, builder.Environment.ContentRootPath, builder.Environment.IsDevelopment());

app.Run();
