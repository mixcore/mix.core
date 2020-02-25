namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixPostMedia
    {
        public int MediaId { get; set; }
        public int PostId { get; set; }
        public string Specificulture { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int Position { get; set; }
        public int Priority { get; set; }
        public int Status { get; set; }

        public virtual MixMedia MixMedia { get; set; }
        public virtual MixPost MixPost { get; set; }
    }
}