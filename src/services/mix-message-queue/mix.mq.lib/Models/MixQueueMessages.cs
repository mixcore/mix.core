using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Mix.Mq.Lib.Models
{
    public class MixQueueMessages<T>
        where T : MessageQueueModel
    {
        private readonly ConcurrentQueue<MixTopicModel<T>> _topics = new();

        public MixQueueMessages()
        {
        }
        public List<MixTopicModel<T>> GetAllTopic()
        {
            return _topics.ToList();
        }
        public MixTopicModel<T> GetTopic(string topicId)
        {
            if (_topics.All(m => m.Id != topicId))
            {
                _topics.Enqueue(new MixTopicModel<T>()
                {
                    Id = topicId
                });
            }
            return _topics.First(m => m.Id == topicId);
        }
    }
}
