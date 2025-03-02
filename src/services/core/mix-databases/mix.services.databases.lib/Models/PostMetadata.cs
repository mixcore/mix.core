using Mix.Mixdb.ViewModels;

namespace Mix.Services.Databases.Lib.Models
{
    public class PostMetadata
    {
        public string MetadataType { get; set; }
        public IEnumerable<MixMetadataViewModel> Data { get; set; }
    }
}
