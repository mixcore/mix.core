using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Mix.Lib.Interfaces;
using Mix.Heart.Extensions;
using System.Reflection;
using Microsoft.OpenApi.Models;

namespace Mix.Lib.Abstracts
{
    public abstract class StartupApi : IStartupService
    {
        protected static string Title { get; set; }
        protected static string Version { get; set; }
        protected static string SwaggerBasePath { get; set; }

        public StartupApi(Assembly assembly)
        {
            Title = assembly.ManifestModule.Name.Replace(".dll", string.Empty);
            Version = "v2";
            SwaggerBasePath = $"api/{Version}/{Title.Replace(".","-").ToHypenCase()}";
        }

        public virtual void AddServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(Version, new OpenApiInfo { Title = Title, Version = Version });
            });
        }

        public virtual void UseApps(IApplicationBuilder app, bool isDevelop)
        {
            if (isDevelop)
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger(opt => opt.RouteTemplate = SwaggerBasePath + "/swagger/{documentName}/swagger.json");
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint($"/{SwaggerBasePath}/swagger/{Version}/swagger.json", $"{Title} {Version}");
                    c.RoutePrefix = $"{SwaggerBasePath}/swagger";
                });
            }
        }
    }
}
