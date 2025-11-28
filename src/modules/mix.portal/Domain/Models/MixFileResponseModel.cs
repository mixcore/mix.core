namespace Mix.Portal.Domain.Models
{
    public sealed class MixFileResponseModel
    {
        public List<FileModel> Files { get; set; }

        public List<string> Directories { get; set; }
    }
}
