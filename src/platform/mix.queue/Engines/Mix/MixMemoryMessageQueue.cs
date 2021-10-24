using Mix.Queue.Models;
using System.Collections.Generic;
using System.Linq;

namespace Mix.Queue.Engines.MixQueue
{
    public class MixMemoryMessageQueue<T>
    {
        private List<MixTopicModel> topics = new List<MixTopicModel>();

        public MixMemoryMessageQueue()
        {
        }

        public MixTopicModel GetTopic(string topicId)
        {
            if (!topics.Any(m => m.Id == topicId))
            {
                topics.Add(new MixTopicModel()
                {
                    Id = topicId
                });
            }
            return topics.Find(m => m.Id == topicId);
        }
    }
}
