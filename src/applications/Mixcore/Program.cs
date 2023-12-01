using Microsoft.Extensions.DependencyInjection.Extensions;
using Mix.Database.Entities.Account;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Mix.Log.Lib;
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

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
