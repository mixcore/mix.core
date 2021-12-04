using Mix.Heart.Models;

namespace Mix.Portal.Domain.Models
{
    public class MixFileResponseModel
    {
        public List<FileModel> Files { get; set; }

        public List<string> Directories { get; set; }
    }
}
