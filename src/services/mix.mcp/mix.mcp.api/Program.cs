using Microsoft.Extensions.DependencyInjection.Extensions;
using Mix.MCP.Lib.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddControllers();
builder.AddMixCommonServices();

builder.AddMCPServices();
builder.AddMixCors();
var app = builder.Build();

app.MapDefaultEndpoints();
app.UseEndpoints(e => e.MapMCPEndpoints(builder.Environment.IsDevelopment()));
// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
