using Mix.Lib.Repositories;
using Mix.Queue.Engines;
using Mix.Queue.Engines.MixQueue;

namespace Mixcore.Domain.Subscribers
{
    public sealed class TenantSubscriber : SubscriberBase
    {
        private UnitOfWorkInfo _uow;
        static string topicId = typeof(MixTenantSystemViewModel).FullName;
        public TenantSubscriber(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixMemoryMessageQueue<MessageQueueModel> queueService)
            : base(topicId, MixModuleNames.Mixcore, configuration, queueService)
        {
            _uow = new(new MixCmsContext(httpContextAccessor));
        }

        public override async Task Handler(MessageQueueModel data)
        {
            var _repository = MixTenantSystemViewModel.GetRepository(_uow);
            var post = data.ParseData<MixTenantSystemViewModel>();
            switch (data.Action)
            {
                case "Get":
                    break;
                case "Post":
                case "Put":
                case "Patch":
                case "Delete":
                    MixTenantRepository.Instance.AllTenants = await _repository.GetAllAsync(m => true);
                    break;
                default:
                    break;
            }
            await _uow.CompleteAsync();
        }
    }
}
