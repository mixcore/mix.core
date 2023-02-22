using Newtonsoft.Json.Linq;

namespace Mix.Database.Entities.Base
{
    public abstract class ExtraColumnMultilingualSEOContentBase<TPrimaryKey> : MultilingualSEOContentBase<TPrimaryKey>
        where TPrimaryKey : IComparable
    {
        public string MixDatabaseName { get; set; }
        public int? MixDbId { get; set; }
        //public JObject? ExtraData { get; set; }
    }
}
