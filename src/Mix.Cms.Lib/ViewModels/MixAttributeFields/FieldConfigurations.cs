using Newtonsoft.Json;

namespace Mix.Cms.Lib.ViewModels.MixAttributeFields
{
    public class FieldConfigurations
    {
        [JsonProperty("upload")]
        public UploadConfigurations Upload { get; set; } = new UploadConfigurations();
    }

    public class UploadConfigurations
    {
        [JsonProperty("width")]
        public int? Width { get; set; }
        [JsonProperty("height")]
        public int? Height { get; set; }
        [JsonProperty("isCrop")]
        public bool IsCrop { get; set; }
    }
}
