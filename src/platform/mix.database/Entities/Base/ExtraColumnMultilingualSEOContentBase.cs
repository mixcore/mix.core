namespace Mix.Database.Entities.Base
{
    public abstract class ExtraColumnMultilingualSEOContentBase<TPrimaryKey> : MultilingualSEOContentBase<TPrimaryKey>
        where TPrimaryKey : IComparable
    {
        public string MixDbTableName { get; set; }
        public int? MixDbTableId { get; set; }
    }
}
