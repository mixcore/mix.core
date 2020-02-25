namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixPortalPageNavigation
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int Priority { get; set; }
        public int Status { get; set; }

        public virtual MixPortalPage IdNavigation { get; set; }
        public virtual MixPortalPage Parent { get; set; }
    }
}