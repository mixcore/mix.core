namespace Mix.Database.Entities.Base
{
    public abstract class MultilingualUniqueNameContentBase<TPrimaryKey> : MultilingualContentBase<TPrimaryKey>
        where TPrimaryKey : IComparable
    {
        public string DisplayName { get; set; }
        public string SystemName { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
    }
}
