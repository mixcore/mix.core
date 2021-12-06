using Mix.Database.Entities.Base;

namespace Mix.Database.Entities.Cms
{
    public class MixModuleData : MultilanguageSEOContentBase<int>
    {
        public string SimpleDataColumns { get; set; }
        public string Value { get; set; }

        public int ModuleContentId { get; set; }
        public virtual MixModuleContent MixModuleContent { get; set; }
    }
}
