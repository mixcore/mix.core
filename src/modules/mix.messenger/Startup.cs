using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
        public void ConfigureServices(IHostApplicationBuilder builder)
        {
            builder.AddMixCors();
            builder.Services.AddMixDbContexts();
            builder.Services.AddMixCache(Configuration);

            string? azureConnectionString = builder.Configuration.GetSection("Azure")["SignalRConnectionString"];
            string? redisConnectionString = builder.Configuration.GetSection("Redis")["ConnectionStrings"];
            builder.AddMixSignalR(azureConnectionString, redisConnectionString);

            builder.Services.AddMixCommunicators();

            UnitOfWorkMiddleware.AddUnitOfWork<UnitOfWorkInfo<MixCacheDbContext>>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMixCors(Configuration);
            app.UseRouting();
            app.UseEndpoints(enpoints => enpoints.UseMixSignalRApp());
        }
    }
}
