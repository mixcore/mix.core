using Mix.Heart.Entities;

namespace Mix.Database.Entities.MixDb
{
    public class MixMedia : EntityBase<int>
    {
        public string Title { get; set; }
        public string Type { get; set; }
        public string FileUrl { get; set; }
    }
}
