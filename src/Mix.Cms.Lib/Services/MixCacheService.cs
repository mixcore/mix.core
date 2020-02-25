using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Repositories;
using Mix.Domain.Data.Repository;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.Services
{
    public class MixCacheService
    {
        /// <summary>
        /// The synchronize root
        /// </summary>
        private static readonly object syncRoot = new Object();

        /// <summary>
        /// The instance
        /// </summary>
        private static volatile MixCacheService instance;

        public DefaultModelRepository<MixCmsContext, MixCache> Repository;

        public MixCacheService()
        {
            Repository = DefaultModelRepository<MixCmsContext, MixCache>.Instance;
        }

        public static MixCacheService Instance {
            get {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new MixCacheService();
                        }
                    }
                }

                return instance;
            }
        }

        public static Task<T> GetAsync<T>(string key)
        {
            var data = FileRepository.Instance.GetFile(key, ".json", MixConstants.Folder.CacheFolder, false, "{}");
            if (data != null && !string.IsNullOrEmpty(data.Content))
            {
                var jobj = JObject.Parse(data.Content);
                return Task.FromResult(jobj.ToObject<T>());
            }
            return Task.FromResult(default(T));

            //var result = await Instance.Repository.GetSingleModelAsync(c => c.Id == key);
            //if (result.IsSucceed)
            //{
            //    var  jobj= JObject.Parse(result.Data.Value);
            //    return jobj.ToObject<T>();
            //}
            //return default(T);
        }

        //public static Task<RepositoryResponse<bool>> SetAsync<T>(string key, T value)
        //{
        //    if (value != null)
        //    {
        //        var jobj = JObject.FromObject(value);
        //        var cacheFile = new FileViewModel()
        //        {
        //            Filename = key.ToLower(),
        //            Extension = ".json",
        //            FileFolder = "Cache",
        //            Content = jobj.ToString(Newtonsoft.Json.Formatting.None)
        //        };

        //        var result = FileRepository.Instance.SaveFile(cacheFile);
        //        return Task.FromResult(new RepositoryResponse<bool>()
        //        {
        //            IsSucceed = result,
        //        });
        //    }
        //    else
        //    {
        //        return Task.FromResult(new RepositoryResponse<bool>());
        //    }
        //    //var getData = await Instance.Repository.GetSingleModelAsync(c => c.Id == key);
        //    //MixCache data = null;
        //    //if (value != null)
        //    //{
        //    //    if (getData.IsSucceed)
        //    //    {
        //    //        data = getData.Data;
        //    //    }
        //    //    else
        //    //    {
        //    //        data = new MixCache();
        //    //    }

        //    //    var jobj = JObject.FromObject(value);
        //    //    data.Id = key;
        //    //    data.Status = (int)MixEnums.MixContentStatus.Published;
        //    //    data.Value = jobj.ToString(Newtonsoft.Json.Formatting.None);
        //    //    data.CreatedDateTime = DateTime.UtcNow;
        //    //    var tmp = Instance.Repository.SaveModel(data);
        //    //    return new RepositoryResponse<bool>()
        //    //    {
        //    //        IsSucceed = tmp.IsSucceed,
        //    //        Exception = tmp.Exception,
        //    //        Errors = tmp.Errors
        //    //    };
        //    //}
        //    //else
        //    //{
        //    //    return new RepositoryResponse<bool>();
        //    //}
        //}

        public static Task RemoveCacheAsync()
        {
            return Task.FromResult(FileRepository.Instance.EmptyFolder(MixConstants.Folder.CacheFolder));
            //await Instance.Repository.RemoveListModelAsync(c => !string.IsNullOrEmpty(c.Id));
        }
    }
}