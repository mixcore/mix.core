using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mix.Constant.Constants;
using Mix.Queue.Engines;
using Mix.Queue.Engines.MixQueue;
using Mix.Queue.Models;
using Mix.RepoDb.Services;

namespace Mix.RepoDb.Sbuscribers
{
    public class MixRepoDbSubscriber : SubscriberBase
    {
        private IServiceProvider _servicesProvider;
        public MixRepoDbSubscriber(
            IConfiguration configuration,
            MixMemoryMessageQueue<MessageQueueModel> queueService, IServiceProvider servicesProvider)
            : base(MixQueueTopics.MixRepoDb, string.Empty, configuration, queueService)
        {
            _servicesProvider = servicesProvider;
        }

        public override async Task Handler(MessageQueueModel model)
        {
            switch (model.Action)
            {
                case MixRepoDbQueueAction.Backup:
                    await BackupDatabase(model);
                    break;
                default:
                    break;
            }
        }

        private async Task BackupDatabase(MessageQueueModel model)
        {
            using (var scope = _servicesProvider.CreateScope())
            {
                var service =
                    scope.ServiceProvider
                        .GetRequiredService<MixDbService>();
                await service.BackupDatabase(model.Data);
            }
        }
    }
}
