using Newtonsoft.Json.Linq;
using System.Linq;

namespace Mix.Theme.Domain.Models
{
    public class ColumnConfigurations
    {
        public int? MaxLength { get; set; }
        public string BelongTo { get; set; }
        public string OptionsConfigurationName { get; set; }
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
