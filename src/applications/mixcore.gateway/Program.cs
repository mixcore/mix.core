
using Microsoft.Extensions.DependencyInjection.Extensions;
using Mix.Constant.Constants;
using Mix.Database.Entities.Cms;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Heart.Services;
using Mix.Lib.Extensions;
using Mix.Lib.Helpers;
using Mix.Lib.Services;
using Mix.Shared.Extensions;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Reflection;


bool isInit = true;

var builder = MixCmsHelper.CreateWebApplicationBuilder(args);
builder.AddServiceDefaults();

builder.WebHost.UseContentRoot(Directory.GetCurrentDirectory());
builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
                       .AddJsonFile("appsettings.json", true, true)
                       .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
                       .AddJsonFile($"{MixFolders.AppConfigFolder}/ocelot.json", true, true)
                       .AddJsonFile($"{MixFolders.AppConfigFolder}/ocelot.{builder.Environment.EnvironmentName}.json", true, true)
                       .AddEnvironmentVariables();
builder.Services.AddOutputCache();
builder.Services.AddControllers();
builder.AddConfigurations();
builder.AddMixServices(Assembly.GetExecutingAssembly());
builder.Services.AddMixAuthorize<MixCmsContext>(builder.Configuration);
builder.Services.AddScoped<MixIdentityService>();
builder.Services.AddOcelot(builder.Configuration);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
if (!builder.Environment.IsLocal())
{
    builder.Services.AddEndpointsApiExplorer();
}
if (isInit)
{
    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(builder =>
        {
            builder.AllowAnyOrigin();
            builder.AllowAnyHeader();
            builder.AllowAnyMethod();
        });
    });
}
else
{
    builder.AddMixCors();
}

var app = builder.Build();
app.MapDefaultEndpoints();

Configure(app, builder.Environment, builder.Configuration);

app.UseOutputCache();
app.MapDefaultEndpoints();
app.Run();

// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
static void Configure(WebApplication app, IWebHostEnvironment env, IConfiguration configuration)
{
    if (!env.IsDevelopment())
    {
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
        app.UseHttpsRedirection();
    }
    //app.UseResponseCompression();
    app.UseRouting();
    if (configuration.IsInit())
    {
        app.UseCors();
    }
    else
    {
        app.UseMixCors(configuration);
    }
    // ref: app.UseMixAuth();
    app.UseMixAuth();

    app.UseMixSwaggerApps(!app.Environment.IsProduction(), Assembly.GetExecutingAssembly());
    app.MapControllers();
    app.UseOcelot().Wait();
}

