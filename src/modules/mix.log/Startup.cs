using Mix.Database.Entities.Account;
using Mix.Lib.Middlewares;
using Mix.Log.Lib;
using System.Reflection;

namespace Mix.Log
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
            services.AddMixServices(Assembly.GetExecutingAssembly(), Configuration);
            services.AddMixCors();
            services.AddMixAuthorize<MixCmsAccountContext>(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMixCors();
            app.UseMixTenant();
            app.UseRouting();
            app.UseMixAuth();
            // auditlog middleware must go after auth
            app.UseMiddleware<AuditlogMiddleware>();
            app.UseMixCors();
            app.UseRouting();
            app.UseMixApps(Assembly.GetExecutingAssembly(), Configuration, env.ContentRootPath, env.IsDevelopment());
        }
    }
}
