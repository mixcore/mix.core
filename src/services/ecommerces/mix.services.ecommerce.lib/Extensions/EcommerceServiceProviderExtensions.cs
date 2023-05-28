using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Middlewares;
using Mix.Services.Ecommerce.Lib.Entities.Mix;
using Mix.Services.Ecommerce.Lib.Entities.Onepay;
using Mix.Services.Ecommerce.Lib.Entities.Paypal;
using Mix.Services.Ecommerce.Lib.Interfaces;
using Mix.Services.Ecommerce.Lib.Services;
using Mix.Shared.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceProviderExtensions
    {
        public static void AddMixEcommerce(this IServiceCollection services)
        {
            services.AddMixOnepay();
            services.AddMixPaypal();
            services.TryAddScoped<EcommerceDbContext>();
            services.TryAddScoped<UnitOfWorkInfo<EcommerceDbContext>>();
            services.TryAddScoped<IOrderService, OrderService>();
            services.TryAddScoped<IEcommerceService, EcommerceService>();
            UnitOfWorkMiddleware.AddUnitOfWork<UnitOfWorkInfo<EcommerceDbContext>>();
        }
        public static void AddMixOnepay(this IServiceCollection services)
        {
            services.TryAddScoped<OnepayDbContext>();
            services.TryAddScoped<UnitOfWorkInfo<OnepayDbContext>>();

            UnitOfWorkMiddleware.AddUnitOfWork<UnitOfWorkInfo<OnepayDbContext>>();
            
            services.TryAddScoped<PaypalDbContext>();
            services.TryAddScoped<UnitOfWorkInfo<PaypalDbContext>>();

            UnitOfWorkMiddleware.AddUnitOfWork<UnitOfWorkInfo<PaypalDbContext>>();
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
        public static void AddMixPaypal(this IServiceCollection services)
        {
            services.TryAddScoped<PaypalDbContext>();
            services.TryAddScoped<UnitOfWorkInfo<PaypalDbContext>>();

            UnitOfWorkMiddleware.AddUnitOfWork<UnitOfWorkInfo<PaypalDbContext>>();
            if (!GlobalConfigService.Instance.IsInit)
            {
                using (var context = services.GetService<PaypalDbContext>())
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