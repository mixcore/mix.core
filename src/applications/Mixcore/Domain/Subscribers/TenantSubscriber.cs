using Grpc.Core;
using Microsoft.AspNetCore.Http;
using Mix.Database.Services;
using Mix.Lib.Repositories;
using Mix.Lib.Services;
using Mix.Queue.Engines;
using Mix.Queue.Engines.MixQueue;

namespace Mixcore.Domain.Subscribers
{
    public sealed class TenantSubscriber : SubscriberBase
    {
        static string topicId = typeof(MixTenantSystemViewModel).FullName;
        private MixTenantService _mixTenantService;
        private readonly DatabaseService _databaseService;
        public IHttpContextAccessor _httpContextAccessor { get; }

        public TenantSubscriber(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixMemoryMessageQueue<MessageQueueModel> queueService,
            MixTenantService mixTenantService,
            DatabaseService databaseService)
            : base(topicId, MixModuleNames.Mixcore, configuration, queueService)
        {
            _httpContextAccessor = httpContextAccessor;
            _mixTenantService = mixTenantService;
            _databaseService = databaseService;
        }

        public override async Task Handler(MessageQueueModel data)
        {
            UnitOfWorkInfo<MixCmsContext> _uow = new(new MixCmsContext(_databaseService));
            switch (data.Action)
            {
                case "Get":
                    break;
                case "Post":
                case "Put":
                case "Patch":
                case "Delete":
                    await _mixTenantService.Reload(_uow);
                    break;
                default:
                    break;
            }
            _ = _uow.DisposeAsync();
        }
    }
}
