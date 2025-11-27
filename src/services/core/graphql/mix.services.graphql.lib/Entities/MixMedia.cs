using Mix.Heart.Entities;

namespace Mix.Services.Graphql.Lib.Entities
{
    public class MixMedia : EntityBase<Guid>
    {
        public string? Title { get; set; }
        public string? Type { get; set; }
        public string? FileUrl { get; set; }
    }
}
