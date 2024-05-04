using Mix.Database.Entities.Account;
using Mix.Lib.Middlewares;
using Mix.Lib.Services;
using Mix.Log.Lib;
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

builder.Services.AddMixServices(Assembly.GetExecutingAssembly(), builder.Configuration);
builder.Services.AddMixCors();
builder.Services.AddMixLog(builder.Configuration);
// Must app Auth config after Add mixservice to init App config 
builder.Services.AddMixAuthorize<MixCmsAccountContext>(builder.Configuration);
builder.Services.AddScoped<MixIdentityService>();

var app = builder.Build();

app.UseMixTenant();
app.UseMiddleware<AuditlogMiddleware>();
app.UseMixCors();
app.UseRouting();
app.UseMixAuth();
app.UseMixCors();
app.UseMixApps(Assembly.GetExecutingAssembly(), builder.Configuration, builder.Environment.ContentRootPath, builder.Environment.IsDevelopment());

app.Run();
