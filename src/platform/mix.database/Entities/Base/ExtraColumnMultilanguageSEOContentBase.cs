using Mix.Database.Entities.Cms;
using System;

namespace Mix.Database.Entities.Base
{
    public abstract class ExtraColumnMultilanguageSEOContentBase<TPrimaryKey> : MultilanguageSEOContentBase<TPrimaryKey>
        where TPrimaryKey : IComparable
    {
        public string MixDatabaseName { get; set; }
        public Guid? MixDataContentId { get; set; }
        public MixDataContent MixDataContent { get; set; }
    }
}
