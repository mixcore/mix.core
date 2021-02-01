using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace Mix.Cms.Lib.ViewModels.MixAttributeFields
{
    public class FieldConfigurations
    {
        [JsonProperty("upload")]
        public UploadConfigurations Upload { get; set; } = new UploadConfigurations();
    }

    public class UploadConfigurations
    {
        [JsonProperty("arrayAccepts")]
        public JArray ArrayAccepts { get; set; } = new JArray();
        [JsonProperty("accepts")]
        public string Accepts { get => string.Join(",", ArrayAccepts.Select(m => m.Value<string>("text")).ToArray()); }
        [JsonProperty("width")]
        public int? Width { get; set; }
        [JsonProperty("height")]
        public int? Height { get; set; }
        [JsonProperty("isCrop")]
        public bool IsCrop { get; set; }
    }
}
