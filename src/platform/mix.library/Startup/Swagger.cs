using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Models;
using Mix.Lib.Filters;
using Mix.Lib.Extensions;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.IO;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        // Swagger must be after AddMvc()
        public static IServiceCollection AddMixSwaggerServices(this IServiceCollection services, Assembly assembly)
        {
            string title = assembly.ManifestModule.Name.Replace(".dll", string.Empty);
            // Convert title to hyphen-case format if needed
            title = title.Contains(" ") ? title : title;
            string version = "v2";
            string swaggerBasePath = string.Empty;

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(version, new OpenApiInfo
                {
                    Title = title,
                    Version = version,
                    Description = "MixCore API endpoints documentation",
                    Contact = new OpenApiContact
                    {
                        Name = "Mixcore",
                        Url = new Uri("https://mixcore.org")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Mixcore License",
                        Url = new Uri("https://github.com/mixcore/mix.core/blob/master/LICENSE")
                    }
                });

                c.OperationFilter<SwaggerFileOperationFilter>();
                c.CustomSchemaIds(x => x.FullName);

                // Include XML comments for all assemblies in the API
                var xmlFile = $"{assembly.GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }

                // Also add XML comments from Mix.Lib
                var libXmlFile = "Mix.Lib.xml";
                var libXmlPath = Path.Combine(AppContext.BaseDirectory, libXmlFile);
                if (File.Exists(libXmlPath))
                {
                    c.IncludeXmlComments(libXmlPath);
                }

                // Add operation filters for standardizing responses
                c.OperationFilter<SwaggerDefaultResponsesFilter>();

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

                // Configure parameter descriptions
                c.DescribeAllParametersInCamelCase();
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
            app.UseSwagger(opt =>
            {
                opt.RouteTemplate = routeTemplate;
                opt.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                {
                    swaggerDoc.Servers = new List<OpenApiServer> {
                            new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}" }
                    };
                });
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
                c.DisplayRequestDuration();
                c.DefaultModelsExpandDepth(0); // Hide schemas section by default
            });
            //}
            return app;
        }

    }
}
