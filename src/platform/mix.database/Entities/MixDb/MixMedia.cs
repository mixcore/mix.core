namespace Mix.Database.Entities.MixDb
{
    public class MixDbMedia : EntityBase<Guid>
    {
        public string Extension { get; set; }
        public string FileFolder { get; set; }
        public string FileName { get; set; }
        public string FileProperties { get; set; }
        public long FileSize { get; set; }
        public string FileType { get; set; }
        public string Title { get; set; }
        public string Tags { get; set; }
        public string Source { get; set; }
        public string TargetUrl { get; set; }
    }
}
