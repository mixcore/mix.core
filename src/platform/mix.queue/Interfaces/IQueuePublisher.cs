using Mix.Queue.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mix.Queue.Interfaces
{
    public interface IQueuePublisher<T>
        where T : MessageQueueModel
    {
        Task SendMessage(T message);

        Task SendMessages(IList<T> messages);
    }
}
