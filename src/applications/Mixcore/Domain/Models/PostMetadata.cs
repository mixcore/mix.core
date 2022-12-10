using Mix.Services.Databases.Lib.ViewModels;

namespace Mixcore.Domain.Models
{
    public class PostMetadata
    {
        public string MetadataType { get; set; }
        public IEnumerable<MixMetadataViewModel> Data { get; set; }
    }
}
