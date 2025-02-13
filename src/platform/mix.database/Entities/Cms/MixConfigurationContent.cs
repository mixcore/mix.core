namespace Mix.Database.Entities.Cms
{
    public class MixConfigurationContent : MultilingualUniqueNameContentBase<int>
    {
        public int? MixConfigurationId { get; set; }
        public string DefaultContent { get; set; }
        public string Category { get; set; }
        public MixDataType DataType { get; set; }
        public virtual MixConfiguration MixConfiguration { get; set; }
    }
}
