using Mix.Mq.Lib.Models;
using Mix.RepoDb.Interfaces;
using Mix.RepoDb.ViewModels;

namespace Mix.Lib.Subscribers.Handlers.MixViewModelChangedHandlers
{
    public class MixDatabaseColumnViewModelHandler
    {

        public static async Task MessageQueueHandler(MessageQueueModel data, IMixDbService mixDbService)
        {
            switch (data.Action)
            {
                case "Get":
                    break;
                case "Post":
                    var repoCol = new RepoDbMixDatabaseColumnViewModel();
                    ReflectionHelper.Map(data.ParseData<MixDatabaseColumnViewModel>(), repoCol);
                    await mixDbService.AddColumn(repoCol);
                    break;
                case "Put":
                case "Patch":
                case "Delete":
                default:
                    break;
            }
        }
    }
}
