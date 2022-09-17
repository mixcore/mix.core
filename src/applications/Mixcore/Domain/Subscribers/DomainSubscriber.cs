using Mix.Lib.Repositories;
using Mix.Queue.Engines;
using Mix.Queue.Engines.MixQueue;

namespace Mixcore.Domain.Subscribers
{
    public class DomainSubscriber : SubscriberBase
    {
        private UnitOfWorkInfo _uow;
        protected IHttpContextAccessor _httpContextAccessor;
        static string topicId = typeof(MixDomainViewModel).FullName;
        public DomainSubscriber(
            IConfiguration configuration,
            MixMemoryMessageQueue<MessageQueueModel> queueService,
            IHttpContextAccessor httpContextAccessor)
            : base(topicId, MixModuleNames.Mixcore, configuration, queueService)
        {
            _httpContextAccessor = httpContextAccessor;
            _uow = new(new MixCmsContext(_httpContextAccessor));
        }

        public override async Task Handler(MessageQueueModel data)
        {
            var _repository = MixTenantSystemViewModel.GetRepository(_uow);
            var post = data.ParseData<MixDomainViewModel>();
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
