using Microsoft.Extensions.DependencyInjection.Extensions;
using Mix.Automation.Lib.Extensions;
using Mix.Lib.Interfaces;
using Mix.Lib.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.AddConfigurations();

builder.Services.AddControllers();
builder.AddMixCommonServices();

builder.AddMixAutomationServices();
builder.AddMixCors();
var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
