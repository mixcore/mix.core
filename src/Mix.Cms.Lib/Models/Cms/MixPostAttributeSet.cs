namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixPostAttributeSet
    {
        public int PostId { get; set; }
        public string Specificulture { get; set; }
        public int AttributeSetId { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int Priority { get; set; }
        public int Status { get; set; }

        public virtual MixAttributeSet AttributeSet { get; set; }
        public virtual MixPost MixPost { get; set; }
    }
}