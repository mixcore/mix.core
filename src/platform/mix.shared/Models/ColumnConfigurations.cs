using Newtonsoft.Json.Linq;

namespace Mix.Shared.Models
{
    [Serializable]
    public class ColumnConfigurations
    {
        public ColumnConfigurations()
        {
        }

        public bool IsRequire { get; set; }
        public bool IsEncrypt { get; set; }
        public bool IsSelect { get; set; }
        public int? MaxLength { get; set; }
        public string BelongTo { get; set; }
        public string OptionsConfigurationName { get; set; }
        public List<object> AllowedValues { get; set; }
        public UploadConfigurations Upload { get; set; } = new UploadConfigurations();
    }

    public class UploadConfigurations
    {
        public JArray ArrayAccepts { get; set; } = new JArray();

        public string Accepts { get => string.Join(",", ArrayAccepts.Select(m => m.Value<string>("text")).ToArray()); }

        public int? Width { get; set; }

        public int? Height { get; set; }
        public bool IsCrop { get; set; }
    }
}
