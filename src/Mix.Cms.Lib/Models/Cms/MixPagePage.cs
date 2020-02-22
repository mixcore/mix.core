namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixPagePage
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Specificulture { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int Priority { get; set; }
        public int Status { get; set; }

        public virtual MixPage MixPage { get; set; }
        public virtual MixPage MixPageNavigation { get; set; }
    }
}