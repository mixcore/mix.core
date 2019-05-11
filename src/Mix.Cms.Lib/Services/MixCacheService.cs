using System;
using System.Threading.Tasks;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.Repository;
using Newtonsoft.Json.Linq;

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

        public static MixCacheService Instance
        {
            get
            {
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

        public static async Task<T> GetAsync<T>(string key){
            var result = await Instance.Repository.GetSingleModelAsync(c => c.Id == key);
            if (result.IsSucceed)
            {
                var  jobj= JObject.Parse(result.Data.Value);
                return jobj.ToObject<T>();
            }
            return default(T);
        }

        public static async Task<RepositoryResponse<bool>> SetAsync<T>(string key, T value){
            var getData = await Instance.Repository.GetSingleModelAsync(c => c.Id == key);
            MixCache data = null;
            if (value != null)
            {

                if (getData.IsSucceed)
                {
                    data = getData.Data;
                }
                else
                {
                    data = new MixCache();
                }

                var jobj = JObject.FromObject(value);
                data.Id = key;
                data.Status = (int)MixEnums.MixContentStatus.Published;
                data.Value = jobj.ToString(Newtonsoft.Json.Formatting.None);
                data.CreatedDateTime = DateTime.UtcNow;
                var tmp = Instance.Repository.SaveModel(data);
                return new RepositoryResponse<bool>()
                {
                    IsSucceed = tmp.IsSucceed,
                    Exception = tmp.Exception,
                    Errors = tmp.Errors
                };
            }
            else
            {
                return new RepositoryResponse<bool>();
            }
        }

        public  static async Task RemoveCacheAsync()
        {
            await Instance.Repository.RemoveListModelAsync(c => !string.IsNullOrEmpty(c.Id));
        }
    }
}
