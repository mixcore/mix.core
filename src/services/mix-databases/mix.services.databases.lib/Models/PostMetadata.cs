using Mix.Services.Databases.Lib.ViewModels;

namespace Mix.Services.Databases.Lib.Models
{
    public class PostMetadata
    {
        public string MetadataType { get; set; }
        public IEnumerable<MixMetadataViewModel> Data { get; set; }
    }
}
