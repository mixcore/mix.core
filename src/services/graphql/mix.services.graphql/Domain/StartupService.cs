﻿using Mix.Services.Graphql.Lib;
using Mix.Shared.Interfaces;

namespace Mix.Services.Graphql.Domain
{
    public class StartupService : IStartupService
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddMixGraphQL();
        }

        public void UseApps(IApplicationBuilder app, IConfiguration configuration, bool isDevelop)
        {
            app.UseMixGraphQL();
        }
    }
}
