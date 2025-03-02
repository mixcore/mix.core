using Mix.Mixdb.Interfaces;
using Mix.Mixdb.ViewModels;
using Mix.Mq.Lib.Models;

namespace Mix.Lib.Subscribers.Handlers.MixViewModelChangedHandlers
{
    public class MixDatabaseColumnViewModelHandler
    {
        
        public static async Task MessageQueueHandler(MessageQueueModel data, IMixdbStructure mixDbService)
        {
            switch (data.Action)
            {
                case "Get":
                    break;
                case "Post":
                    var repoCol = new MixdbDatabaseColumnViewModel();
                    ReflectionHelper.Map(data.ParseData<MixDatabaseColumnViewModel>(), repoCol);
                    await mixDbService.AddColumn(repoCol);
                    break;
                case "Put":
                case "PATCH":
                case "DELETE":
                default:
                    break;
            }
        }
    }
}
