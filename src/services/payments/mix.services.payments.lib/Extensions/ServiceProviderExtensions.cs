using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Middlewares;
using Mix.Services.Payments.Lib.Constants.Services;
using Mix.Services.Payments.Lib.Entities.Mix;
using Mix.Services.Payments.Lib.Entities.Onepay;
using Mix.Services.Payments.Lib.Services;
using Mix.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceProviderExtensions
    {
        public static void AddMixPayment(this IServiceCollection services)
        {
            services.TryAddScoped<PaymentDbContext>();
            services.TryAddScoped<UnitOfWorkInfo<PaymentDbContext>>();
            services.TryAddScoped<EcommerceService>();
            UnitOfWorkMiddleware.AddUnitOfWork<UnitOfWorkInfo<PaymentDbContext>>();
        }
        
        public static void AddMixOnepay(this IServiceCollection services)
        {
            services.TryAddScoped<OnepayDbContext>();
            services.TryAddScoped<UnitOfWorkInfo<OnepayDbContext>>();
            services.TryAddScoped<OnepayService>();
            UnitOfWorkMiddleware.AddUnitOfWork<UnitOfWorkInfo<OnepayDbContext>>();
            if (!GlobalConfigService.Instance.IsInit)
            {
                using (var context = services.GetService<OnepayDbContext>())
                {
                    var pendingMigrations = context.Database.GetPendingMigrations();
                    if (pendingMigrations.Any())
                    {
                        context.Database.Migrate();
                    }
                }
            }
        }
    }
}
