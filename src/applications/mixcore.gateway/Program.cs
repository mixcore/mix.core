
using Microsoft.Extensions.DependencyInjection.Extensions;
using Mix.Constant.Constants;
using Mix.Heart.Services;
using Mix.Shared.Services;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Unicode;


bool isInit = true;
if (Directory.Exists("../mixcore/mixcontent/shared"))
{
    isInit = false;
    MixFileHelper.CopyFolder("../mixcore/mixcontent/shared", MixFolders.MixContentSharedFolder);
}

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();
builder.WebHost.UseContentRoot(Directory.GetCurrentDirectory());
builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
                       .AddJsonFile("appsettings.json", true, false)
                       .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
                       .AddJsonFile("ocelot.json", true, false)
                       .AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", true, true)
                       .AddEnvironmentVariables();
builder.Services.AddOutputCache();
builder.Services.AddControllers();
builder.Services.TryAddSingleton<MixEndpointService>();
builder.Services.AddOcelot(builder.Configuration);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
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
    builder.Services.AddMixCors();
}
builder.Services.AddSwaggerGen();

var app = builder.Build();

Configure(app, builder.Environment, isInit);

app.UseOutputCache();
app.MapDefaultEndpoints();
app.Run();

// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
static void Configure(WebApplication app, IWebHostEnvironment env, bool isInit)
{
    if (!env.IsDevelopment())
    {
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }
    //app.UseResponseCompression();
    app.UseRouting();
    if (isInit)
    {
        app.UseCors();
    }
    else
    {
        app.UseMixCors();
    }
    // ref: app.UseMixAuth();
    app.UseAuthentication();
    app.UseAuthorization();

    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapControllers();
    app.UseOcelot().Wait();
}

