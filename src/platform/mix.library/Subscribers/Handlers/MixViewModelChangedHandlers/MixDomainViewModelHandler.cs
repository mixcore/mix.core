using Mix.Lib.Interfaces;
using Mix.Mq.Lib.Models;

namespace Mix.Lib.Subscribers.Handlers.MixViewModelChangedHandlers
{
    public class MixDomainViewModelHandler
    {
        public static Task MessageQueueHandler(MessageQueueModel data, IMixTenantService mixTenantService)
        {
            switch (data.Action)
            {
                case "Get":
                    break;
                case "Post":
                case "Put":
                case "Patch":
                case "Delete":
                    return mixTenantService.Reload();
                default:
                    break;
            }
            return Task.CompletedTask;
        }
    }
}
