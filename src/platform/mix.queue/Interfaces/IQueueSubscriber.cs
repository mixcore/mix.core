using System.Threading;
using System.Threading.Tasks;

namespace Mix.Queue.Interfaces
{
    public interface IQueueSubscriber
    {
        public string SubscriptionId { get; set; }
        Task ProcessQueue(CancellationToken cancellationToken = default);
    }
}
