using Mix.Heart.Entities;

namespace Mix.Mixdb.Entities
{
    public class MixMedia : EntityBase<int>
    {
        public int? ProductDetailsId { get; set; }
        public int? ProductVariantId { get; set; }
        public string? Title { get; set; }
        public string? Type { get; set; }
        public string? FileUrl { get; set; }
    }
}
