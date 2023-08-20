using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mix.Heart.Entities.Cache;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Middlewares;

namespace Mix.Messenger
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
            services.AddMixCors();
            services.AddMixDbContexts();
            services.AddMixCache(Configuration);
            services.AddMixSignalR(Configuration);
            services.AddMixCommunicators();

            UnitOfWorkMiddleware.AddUnitOfWork<UnitOfWorkInfo<MixCacheDbContext>>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMixCors();
            app.UseRouting();
            app.UseEndpoints(enpoints => enpoints.UseMixSignalRApp());
        }
    }
}
