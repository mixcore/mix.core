using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Cache.CacheManager;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Reflection;

namespace Mixcore.Gateway
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

            services.AddMvc();
            services.AddMixSwaggerServices(Assembly.GetExecutingAssembly());

            services.AddOcelot(Configuration)
               .AddCacheManager(x =>
               {
                   x.WithDictionaryHandle();
               });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (!env.IsDevelopment())
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //app.UseResponseCompression();
            app.UseRouting();
            app.UseMixAuth();
            app.UseMixSwaggerApps(env.IsDevelopment(), Assembly.GetExecutingAssembly());
            app.UseOcelot().Wait();
        }
    }
}
