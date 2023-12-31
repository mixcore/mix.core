using Mix.Constant.Constants;
using Mix.Database.Entities.Account;
using Mix.Heart.Services;
using System.Configuration;
using System.Reflection;
using Mix.Log.Lib;
using Microsoft.Azure.Amqp.Framing;
using Mix.Lib.Middlewares;

if (Directory.Exists("../../../applications/mixcore/mixcontent"))
{
    MixFileHelper.CopyFolder("../../../applications/mixcore/mixcontent", MixFolders.MixContentFolder);
}
var builder = MixCmsHelper.CreateWebApplicationBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMixServices(Assembly.GetExecutingAssembly(), builder.Configuration);
builder.Services.AddMixCors();
builder.Services.AddMixLog(builder.Configuration);
// Must app Auth config after Add mixservice to init App config 
builder.Services.AddMixAuthorize<MixCmsAccountContext>(builder.Configuration);


var app = builder.Build();

app.MapDefaultEndpoints();

app.UseMixTenant();
app.UseMiddleware<AuditlogMiddleware>();
app.UseMixAuth();
app.UseMixCors();
app.UseRouting();
app.UseMixCors();
app.UseMixApps(Assembly.GetExecutingAssembly(), builder.Configuration, builder.Environment.ContentRootPath, builder.Environment.IsDevelopment());

app.Run();
