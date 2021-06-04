using Mix.Database.Entities.Base;

namespace Mix.Database.Entities.Cms.v2
{
    public class MixModuleData : MultilanguageSEOContentBase<int>
    {
        public string Fields { get; set; }
        public string Value { get; set; }

        public int MixModuleId { get; set; }
        public virtual MixModule MixModule { get; set; }
    }
}
