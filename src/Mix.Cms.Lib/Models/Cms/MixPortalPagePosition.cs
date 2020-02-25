namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixPortalPagePosition
    {
        public int PositionId { get; set; }
        public int PortalPageId { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public int Status { get; set; }

        public virtual MixPortalPage PortalPage { get; set; }
        public virtual MixPosition Position { get; set; }
    }
}