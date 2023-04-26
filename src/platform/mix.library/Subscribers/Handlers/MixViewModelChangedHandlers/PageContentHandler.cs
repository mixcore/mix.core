namespace Mix.Lib.Subscribers.Handlers.MixViewModelChangedHandlers
{
    public class PageContentHandler
    {
        public static Task MessageQueueHandler(MessageQueueModel data, MixCacheService cacheService)
        {
            var template = data.ParseData<MixPageContentViewModel>();
            switch (data.Action)
            {
                case "Get":
                    break;
                case "Post":
                case "Put":
                case "Patch":
                case "Delete":
                    return DeleteCacheAsync(template, cacheService);
                default:
                    break;
            }
            return Task.CompletedTask;
        }
        private static Task DeleteCacheAsync(MixPageContentViewModel data, MixCacheService cacheService)
        {
            return cacheService.RemoveCacheAsync(data.Id.ToString(), typeof(MixPageContent).FullName);
        }
    }
}
