namespace Mix.Database.Entities.Base
{
    public abstract class ExtraColumnMultilingualSEOContentBase<TPrimaryKey> : MultilingualSEOContentBase<TPrimaryKey>
        where TPrimaryKey : IComparable
    {
        public string MixDatabaseName { get; set; }
        public Guid? MixDataContentId { get; set; }
        public MixDataContent MixDataContent { get; set; }
    }
}
