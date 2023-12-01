using Microsoft.Extensions.DependencyInjection.Extensions;
using Mix.Database.Entities.Account;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Mix.Log.Lib;
using Microsoft.Extensions.FileProviders;
using Mix.Lib.Middlewares;
using Mix.Shared.Services;
var builder = MixCmsHelper.CreateWebApplicationBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddWebEncoders(options =>
{
    options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All);
});

builder.Services.AddMixServices(Assembly.GetExecutingAssembly(), builder.Configuration);
builder.Services.AddMixCors();
builder.Services.AddScoped<MixNavigationService>();
builder.Services.AddMixLog(builder.Configuration);
builder.Services.AddMixAuthorize<MixCmsAccountContext>(builder.Configuration);

builder.Services.TryAddScoped<MixcorePostService>();

var app = builder.Build();

Configure(app, builder.Environment, builder.Configuration);

app.Run();


void Configure(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration configuration)
{
    if (!env.IsDevelopment())
    {
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseMixTenant();

    app.UseMiddleware<AuditlogMiddleware>();

    app.UseRouting();

    // Typically, UseStaticFiles is called before UseCors. Apps that use JavaScript to retrieve static files cross site must call UseCors before UseStaticFiles.
    app.UseMixStaticFiles(env.ContentRootPath);
    app.UseStaticFiles();
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(
            Path.Combine(env.ContentRootPath, MixFolders.TemplatesFolder))
    });

    // UseCors must be placed after UseRouting and before UseAuthorization. This is to ensure that CORS headers are included in the response for both authorized and unauthorized calls.
    app.UseMixCors();

    // must go between app.UseRouting() and app.UseEndpoints.
    app.UseMixAuth();

    app.UseMixApps(Assembly.GetExecutingAssembly(), configuration, env.ContentRootPath, env.IsDevelopment());

    app.UseResponseCompression();
    app.UseMixResponseCaching();



    if (GlobalConfigService.Instance.AppSettings.IsHttps)
    {
        app.UseHttpsRedirection();
    }


}