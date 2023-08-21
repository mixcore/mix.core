using Microsoft.AspNetCore.Http;
using Mix.Database.Entities.Queue;
using Mix.Service.Models;
using Newtonsoft.Json.Linq;

namespace Mix.Log.Interfaces
{
    public interface IMixQueueLog
    {
        public Task SaveRequestAsync(MixQueueMessageLog request);
    }
}
