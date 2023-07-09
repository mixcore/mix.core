using Mix.Queue.Models;
using System.Collections.Generic;
using System.Linq;

namespace Mix.Queue.Engines.MixQueue
{
    public class MixQueueMessages<T>
    {
        private readonly List<MixTopicModel> _topics = new();

        public MixQueueMessages()
        {
        }
        public List<MixTopicModel> GetAllTopic()
        {
            return _topics;
        }
        public MixTopicModel GetTopic(string topicId)
        {
            if (_topics.All(m => m.Id != topicId))
            {
                _topics.Add(new MixTopicModel()
                {
                    Id = topicId
                });
            }
            return _topics.Find(m => m.Id == topicId);
        }
    }
}
