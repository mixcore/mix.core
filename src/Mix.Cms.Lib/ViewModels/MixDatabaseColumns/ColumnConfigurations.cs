using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace Mix.Cms.Lib.ViewModels.MixDatabaseColumns
{
    public class ColumnConfigurations
    {
        [JsonProperty("maxLength")]
        public int? MaxLength { get; set; }
        [JsonProperty("belongTo")]
        public string BelongTo { get; set; }
        [JsonProperty("optionsConfigurationName")]
        public string OptionsConfigurationName { get; set; }
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