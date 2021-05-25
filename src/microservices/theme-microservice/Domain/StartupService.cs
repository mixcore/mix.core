using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Mix.Lib.Modules;

namespace Mix.theme.Domain
{
    public class StartupService : IStartupService
    {
        private readonly string Title = "Mix Theme";
        private readonly string Version = "v2";
        private readonly string SwaggerBasePath = "api/v2/mix-theme";

        public void AddServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(Version, new OpenApiInfo { Title = Title, Version = Version });
            });
        }

        public void UseApps(IApplicationBuilder app, bool isDevelop)
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
