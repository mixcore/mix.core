using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Models;
using Mix.Lib.Filters;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        // Swagger must be after AddMvc()
        public static IServiceCollection AddMixSwaggerServices(this IServiceCollection services, Assembly assembly)
        {
            string title = assembly.ManifestModule.Name.Replace(".dll", string.Empty).ToHyphenCase(' ');
            string version = "v2";
            string swaggerBasePath = string.Empty;

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
                c.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                    {
                        {securityScheme, new string[] { }}
                    }
                );
            });
            return services;
        }

        // Swagger must be after UseRouting()
        public static IApplicationBuilder UseMixSwaggerApps(this IApplicationBuilder app, bool isDevelop, Assembly assembly)
        {
            string title = assembly.ManifestModule.Name.Replace(".dll", string.Empty);
            string version = "v2";
            string swaggerBasePath = string.Empty;
            string routePrefix = $"swagger";
            string routeTemplate = "swagger/{documentName}/swagger.json";
            string endPoint = $"/swagger/{version}/swagger.json";

            //if (isDevelop)
            //{
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

                app.UseSwagger(opt =>
                {
                    opt.RouteTemplate = routeTemplate;
                });
                app.UseSwaggerUI(c =>
                {
                    c.InjectStylesheet("/mix-app/css/swagger.css");
                    c.InjectJavascript("/mix-app/js/swagger.js");
                    c.SwaggerEndpoint(endPoint, $"{title} {version}");
                    c.RoutePrefix = routePrefix;
                    c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
                    c.DocumentTitle = "Mixcore - API Specification";
                    c.EnableFilter();
                    c.EnableDeepLinking();
                });
            //}
            return app;
        }

    }
}
