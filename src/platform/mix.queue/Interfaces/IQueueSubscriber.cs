using System.Threading;
using System.Threading.Tasks;

namespace Mix.Queue.Interfaces
{
    public interface IQueueSubscriber
    {
        Task ProcessQueue(CancellationToken cancellationToken = default);
    }
}
