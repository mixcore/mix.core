using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Mix.Constant.Constants;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Extensions;
using Mix.Lib.Middlewares;
using Mix.Services.Ecommerce.Lib.Entities.Mix;
using Mix.Services.Ecommerce.Lib.Entities.Onepay;
using Mix.Services.Ecommerce.Lib.Entities.Paypal;
using Mix.Services.Ecommerce.Lib.Interfaces;
using Mix.Services.Ecommerce.Lib.Models;
using Mix.Services.Ecommerce.Lib.Services;
using Mix.Shared.Models.Configurations;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceProviderExtensions
    {
        public static void AddMixEcommerce(this IHostApplicationBuilder builder)
        {
            var paymentSettings = builder.Configuration.GetSection(MixAppSettingsSection.Payments).Get<PaymentConfigurationModel>();
            if (paymentSettings != null && paymentSettings.IsActive)
            {
                builder.AddMixOnepay();
                builder.AddMixPaypal();
                builder.Services.AddEcommerceDb();
                builder.Services.TryAddScoped<IOrderService, OrderService>();
                builder.Services.TryAddScoped<IEcommerceService, EcommerceService>();
            }
        }
        public static void AddEcommerceDb(this IServiceCollection services)
        {
            services.TryAddScoped<EcommerceDbContext>();
            services.TryAddScoped<UnitOfWorkInfo<EcommerceDbContext>>();
            UnitOfWorkMiddleware.AddUnitOfWork<UnitOfWorkInfo<EcommerceDbContext>>();
            //if (!EnvironmentService.IsInit)
            //{
            //    using (var context = builder.GetService<EcommerceDbContext>())
            //    {
            //        var pendingMigrations = context.Database.GetPendingMigrations();
            //        if (pendingMigrations.Any())
            //        {
            //            context.Database.Migrate();
            //        }
            //    }
            //}
        }
        public static void AddMixOnepay(this IHostApplicationBuilder builder)
        {
            builder.Services.TryAddScoped<OnepayDbContext>();
            builder.Services.TryAddScoped<UnitOfWorkInfo<OnepayDbContext>>();
            UnitOfWorkMiddleware.AddUnitOfWork<UnitOfWorkInfo<OnepayDbContext>>();

            if (!builder.Configuration.IsInit())
            {
                using (var context = builder.Services.GetService<OnepayDbContext>())
                {
                    var pendingMigrations = context.Database.GetPendingMigrations();
                    if (pendingMigrations.Any())
                    {
                        context.Database.Migrate();
                    }
                    context.Dispose();
                }
            }
            builder.Services.TryAddScoped<OnepayService>();
        }
        public static void AddMixPaypal(this IHostApplicationBuilder builder)
        {
            builder.Services.TryAddScoped<PaypalDbContext>();
            builder.Services.TryAddScoped<UnitOfWorkInfo<PaypalDbContext>>();

            UnitOfWorkMiddleware.AddUnitOfWork<UnitOfWorkInfo<PaypalDbContext>>();
            if (!builder.Configuration.IsInit())
            {
                using (var context = builder.Services.GetService<PaypalDbContext>())
                {
                    var pendingMigrations = context.Database.GetPendingMigrations();
                    if (pendingMigrations.Any())
                    {
                        context.Database.Migrate();
                    }
                    context.Dispose();  
                }
            }
            builder.Services.TryAddScoped<PaypalService>();
        }
    }
}