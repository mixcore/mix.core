using System.Threading.Tasks;

namespace Mix.Queue.Interfaces
{
    public interface IQueueSubscriber
    {
        Task ProcessQueue();
    }
}
