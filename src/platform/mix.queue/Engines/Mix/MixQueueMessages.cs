using Mix.Queue.Engines.Mix;
using Mix.Queue.Models;
using System.Collections.Generic;
using System.Linq;

namespace Mix.Queue.Engines.MixQueue
{
    public class MixQueueMessages<T>
        where T : MessageQueueModel
    {
        private readonly List<MixTopicModel<T>> _topics = new();

        public MixQueueMessages()
        {
        }
        public List<MixTopicModel<T>> GetAllTopic()
        {
            return _topics;
        }
        public MixTopicModel<T> GetTopic(string topicId)
        {
            if (_topics.All(m => m.Id != topicId))
            {
                _topics.Add(new MixTopicModel<T>()
                {
                    Id = topicId
                });
            }
            return _topics.Find(m => m.Id == topicId);
        }
    }
}
