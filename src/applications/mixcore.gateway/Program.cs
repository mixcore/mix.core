
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Unicode;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.WebHost.UseContentRoot(Directory.GetCurrentDirectory());
builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
                       .AddJsonFile("appsettings.json", true, true)
                       .AddJsonFile("ocelot.json", true, true)
                       .AddEnvironmentVariables();
builder.Services.AddOutputCache();
builder.Services.AddControllers();
builder.Services.AddOcelot(builder.Configuration);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

Configure(app, builder.Environment);
app.UseOutputCache();
app.MapDefaultEndpoints();
app.Run();

// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
static void Configure(WebApplication app, IWebHostEnvironment env)
{
    if (!env.IsDevelopment())
    {
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }
    //app.UseResponseCompression();
    app.UseRouting();

    // ref: app.UseMixAuth();
    app.UseAuthentication();
    app.UseAuthorization();

    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapControllers();
    app.UseOcelot().Wait();
}