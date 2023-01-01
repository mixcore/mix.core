using Mix.Heart.Entities;

namespace Mix.Services.Databases.Lib.Entities
{
    public class MixMedia : EntityBase<int>
    {
        public int? ProductDetailsId { get; set; }
        public string? Title { get; set; }
        public string? Type { get; set; }
        public string? FileUrl { get; set; }
    }
}
