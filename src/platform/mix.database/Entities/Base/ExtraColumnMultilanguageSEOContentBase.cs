using Mix.Database.Entities.Cms;

namespace Mix.Database.Entities.Base
{
    public abstract class ExtraColumnMultilanguageSEOContentBase<TPrimaryKey> : MultiLanguageSEOContentBase<TPrimaryKey>
        where TPrimaryKey : IComparable
    {
        public string MixDatabaseName { get; set; }
        public Guid? MixDataContentId { get; set; }
        public MixDataContent MixDataContent { get; set; }
    }
}
