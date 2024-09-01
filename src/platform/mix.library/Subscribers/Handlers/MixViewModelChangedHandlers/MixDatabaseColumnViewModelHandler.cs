using Mix.Lib.Interfaces;
using Mix.Lib.Services;
using Mix.Mq.Lib.Models;
using Mix.RepoDb.Interfaces;
using Mix.RepoDb.Services;
using Mix.RepoDb.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                case "PATCH":
                case "DELETE":
                default:
                    break;
            }
        }
    }
}
