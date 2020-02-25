namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixPagePosition
    {
        public int PositionId { get; set; }
        public int PageId { get; set; }
        public string Specificulture { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public int Status { get; set; }

        public virtual MixPage MixPage { get; set; }
        public virtual MixPosition Position { get; set; }
    }
}