using Mix.Database.Entities.Base;

namespace Mix.Database.Entities.Cms
{
    public class MixModuleData : MultilingualSEOContentBase<int>
    {
        public string SimpleDataColumns { get; set; }
        public string Value { get; set; }

        public virtual MixModuleContent MixModuleContent { get; set; }
    }
}
