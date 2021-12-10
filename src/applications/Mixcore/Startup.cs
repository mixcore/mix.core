using Mix.Shared.Services;
using System.Reflection;
using Ocelot.DependencyInjection;
using Mixcore.Domain.Subscribers;
using System.Text.Unicode;
using System.Text.Encodings.Web;
using Mix.Database.Entities.Account;
using Mixcore.Domain.Services;

namespace Mixcore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddWebEncoders(options =>
            {
                options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All);
            });

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin();
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                });
            });

            services.AddMixServices(Assembly.GetExecutingAssembly(), Configuration);
            services.AddScoped<MixNavigationService>();
            // Queue Subscribers
            services.AddHostedService<ThemeSubscriberService>();
            services.AddHostedService<TemplateSubscriberService>();
            services.AddHostedService<PageContentSubscriberService>();

            services.AddMixAuthorize<ApplicationDbContext>();
            services.AddMixRoutes();

            // Must app Auth config after Add mixservice to init App config 
            services.AddOcelot(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin();
                builder.AllowAnyMethod();
                builder.AllowAnyHeader();
                builder.WithExposedHeaders("Grpc-Status", "Grpc-Message");
            });
            
            app.UseMixApps(Assembly.GetExecutingAssembly(), Configuration, env.IsDevelopment());

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            if (GlobalConfigService.Instance.AppSettings.EnableOcelot)
            {
                app.UseMixOcelot(Configuration, env.IsDevelopment());
            }
            if (GlobalConfigService.Instance.AppSettings.IsHttps)
            {
                app.UseHttpsRedirection();
            }

            
            app.UseMixRoutes();
        }
    }
}
