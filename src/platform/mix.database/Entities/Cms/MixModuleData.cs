namespace Mix.Database.Entities.Cms
{
    public class MixModuleData : MultilingualSEOContentBase<int>
    {
        public string SimpleDataColumns { get; set; }
        public string Value { get; set; }
        public int MixModuleContentId { get; set; }

        public virtual MixModuleContent MixModuleContent { get; set; }
    }
}
