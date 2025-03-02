using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mix.Mq.Lib.Models;
using Mix.Mq.Server.Domain.Services;
using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace Mix.Mq.Server.Controllers
{

    [Route("api/mq")]
    [ApiController]
    public class MqController : ControllerBase
    {
        private readonly MixQueueMessages<MessageQueueModel> _queue;
        private readonly GrpcStreamingService _grpcStreamingService;

        public MqController(MixQueueMessages<MessageQueueModel> queue, GrpcStreamingService grpcStreamingService)
        {
            _queue = queue;
            _grpcStreamingService = grpcStreamingService;
        }

        [HttpGet("topics")]
        public ActionResult GetQueueMessages()
        {
            var topics = _queue.GetAllTopic();
            return new JsonResult(topics);
        }

        [HttpGet("streams")]
        public ActionResult GetStreams()
        {
            List<object> arr = new();
            foreach (var item in _grpcStreamingService.MessageSubscriptions)
            {
                arr.Add(new
                {
                    subscription_id = item.Key.SubsctiptionId,
                    stream_id = item.Value.GetHashCode()
                });
            }
            return new JsonResult(arr);
        }

        [HttpGet("pending-messages")]
        public ActionResult GetPendingMessages()
        {
            List<object> arr = new();
            foreach (var item in _grpcStreamingService.PendingMessages)
            {
                arr.Add(new
                {
                    subscription_id = item.Key,
                    messages = item.Value
                });
            }
            return new JsonResult(arr);
        }
    }
}
