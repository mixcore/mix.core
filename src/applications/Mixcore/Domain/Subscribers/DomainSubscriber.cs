using Mix.Lib.Repositories;
using Mix.Queue.Engines;
using Mix.Queue.Engines.MixQueue;

namespace Mixcore.Domain.Subscribers
{
    public class DomainSubscriber : SubscriberBase
    {
        private UnitOfWorkInfo _uow;
        static string topicId = typeof(MixDomainViewModel).FullName;
        public DomainSubscriber(
            IConfiguration configuration,
            MixMemoryMessageQueue<MessageQueueModel> queueService)
            : base(topicId, MixModuleNames.Mixcore, configuration, queueService)
        {
            _uow = new(new MixCmsContext());
        }

        public override async Task Handler(MessageQueueModel data)
        {
            var _repository = MixTenantViewModel.GetRepository(_uow);
            var post = data.Data.ToObject<MixDomainViewModel>();
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
