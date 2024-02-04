using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.ObjectPool;
using Mix.Queue.Engines.RabitMQ;
using Mix.Queue.Models.QueueSetting;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Queue.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IHostApplicationBuilder AddMixQueue(this IHostApplicationBuilder builder)
        {
            var providerSetting = builder.Configuration["MessageQueueSetting:Provider"];
            var _provider = Enum.Parse<MixQueueProvider>(providerSetting);
            switch (_provider)
            {
                case MixQueueProvider.GOOGLE:
                    builder.AddGooglePubSub();
                    break;
                case MixQueueProvider.RABBITMQ:
                    builder.AddRabbit();
                    break;
                case MixQueueProvider.AWS:
                    break;
                case MixQueueProvider.AZURE:
                    builder.AddAzureServiceBus();
                    break;
                case MixQueueProvider.MIX:
                    builder.AddMixMQ();
                    break;
                default:
                    break;
            }
            return builder;
        }

        public static IServiceCollection AddGooglePubSub(this IHostApplicationBuilder builder)
        {
            var config = builder.Configuration.GetSection("MessageQueueSetting:GoogleQueueSetting");
            builder.Services.Configure<GoogleQueueSetting>(config);

            return builder.Services;
        }
         public static IServiceCollection AddMixMQ(this IHostApplicationBuilder builder)
        {
            var config = builder.Configuration.GetSection("MessageQueueSetting:Mix");
            builder.Services.Configure<MixQueueSetting>(config);

            return builder.Services;
        }
        
        public static IServiceCollection AddAzureServiceBus(this IHostApplicationBuilder builder)
        {
            var config = builder.Configuration.GetSection("MessageQueueSetting:AzureServiceBus");
            builder.Services.Configure<AzureQueueSetting>(config);

            return builder.Services;
        }
        
        public static IServiceCollection AddRabbit(this IHostApplicationBuilder builder)
        {
            var rabbitConfig = builder.Configuration.GetSection("MessageQueueSetting:RabitMqQueueSetting");
            builder.Services.Configure<RabitMqQueueSetting>(rabbitConfig);

            builder.Services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
            builder.Services.AddSingleton<IPooledObjectPolicy<IModel>, RabbitModelPooledObjectPolicy>();

            return builder.Services;
        }
    }
}
