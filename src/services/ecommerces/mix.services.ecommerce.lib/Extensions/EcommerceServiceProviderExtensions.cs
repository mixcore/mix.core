using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Mix.Constant.Constants;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Middlewares;
using Mix.Services.Ecommerce.Lib.Entities.Mix;
using Mix.Services.Ecommerce.Lib.Entities.Onepay;
using Mix.Services.Ecommerce.Lib.Entities.Paypal;
using Mix.Services.Ecommerce.Lib.Interfaces;
using Mix.Services.Ecommerce.Lib.Models;
using Mix.Services.Ecommerce.Lib.Services;
using Mix.Shared.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceProviderExtensions
    {
        public static void AddMixEcommerce(this IServiceCollection services, Configuration.IConfiguration configuration)
        {
            var paymentSettings = configuration.GetSection(MixAppSettingsSection.Payments).Get<PaymentConfigurationModel>();
            if (paymentSettings != null && paymentSettings.IsActive)
            {
                services.AddMixOnepay();
                services.AddMixPaypal();
                services.AddEcommerceDb();
                services.TryAddScoped<IOrderService, OrderService>();
                services.TryAddScoped<IEcommerceService, EcommerceService>();
            }
        }
        public static void AddEcommerceDb(this IServiceCollection services)
        {
            services.TryAddScoped<EcommerceDbContext>();
            services.TryAddScoped<UnitOfWorkInfo<EcommerceDbContext>>();
            UnitOfWorkMiddleware.AddUnitOfWork<UnitOfWorkInfo<EcommerceDbContext>>();
            //if (!GlobalConfigService.Instance.IsInit)
            //{
            //    using (var context = services.GetService<EcommerceDbContext>())
            //    {
            //        var pendingMigrations = context.Database.GetPendingMigrations();
            //        if (pendingMigrations.Any())
            //        {
            //            context.Database.Migrate();
            //        }
            //    }
            //}
        }
        public static void AddMixOnepay(this IServiceCollection services)
        {
            services.TryAddScoped<OnepayDbContext>();
            services.TryAddScoped<UnitOfWorkInfo<OnepayDbContext>>();
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
            services.TryAddScoped<OnepayService>();
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
            services.TryAddScoped<PaypalService>();
        }
    }
}