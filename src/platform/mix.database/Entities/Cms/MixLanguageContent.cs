namespace Mix.Database.Entities.Cms
{
    public class MixLanguageContent : MultilingualUniqueNameContentBase<int>
    {
        public string DefaultContent { get; set; }
        public string Category { get; set; }
        public MixDataType DataType { get; set; }
        public int MixLanguageId { get; set; }
        public virtual MixLanguage MixLanguage { get; set; }
    }
}
