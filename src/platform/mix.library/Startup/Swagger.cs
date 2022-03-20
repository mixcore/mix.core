using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Models;
using Mix.Lib.Filters;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        private static IServiceCollection AddMixSwaggerServices(this IServiceCollection services, Assembly assembly)
        {
            string title = assembly.ManifestModule.Name.Replace(".dll", string.Empty).ToHypenCase(' ');
            string version = "v2";
            string swaggerBasePath = $"api/{version}/{title.Replace(".", "-").ToHypenCase()}";

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(version, new OpenApiInfo { Title = title, Version = version });
                c.OperationFilter<SwaggerFileOperationFilter>();
                c.CustomSchemaIds(x => x.FullName);

                // add JWT Authentication
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    Description = "Enter JWT Bearer token **_only_**",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer", // must be lower case
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {securityScheme, new string[] { }}
    });
            });
            return services;
        }

        private static IApplicationBuilder UseMixSwaggerApps(this IApplicationBuilder app, bool isDevelop, Assembly assembly)
        {
            string title = assembly.ManifestModule.Name.Replace(".dll", string.Empty);
            string version = "v2";
            string swaggerBasePath = $"api/{version}/{title.Replace(".", "-").ToHypenCase()}";
            string routePrefix = $"{swaggerBasePath}/swagger";
            string routeTemplate = swaggerBasePath + "/swagger/{documentName}/swagger.json";
            string endPoint = $"/{swaggerBasePath}/swagger/{version}/swagger.json";
            if (isDevelop)
            {
                app.UseSwagger(opt => opt.RouteTemplate = routeTemplate);
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint(endPoint, $"{title} {version}");
                    c.RoutePrefix = routePrefix;
                });
            }
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            return app;
        }

    }
}
