using Mix.Shared.Enums;
using Newtonsoft.Json.Linq;

namespace Mix.Lib.Models.Common
{
    public class ModuleFieldModel
    {
        public string Name { get; set; }

        public string Title { get; set; }

        public string DefaultValue { get; set; }

        public JArray Options { get; set; } = new JArray();

        public int Priority { get; set; }

        public MixDataType DataType { get; set; }

        public bool IsUnique { get; set; }

        public bool IsRequired { get; set; }

        public bool IsDisplay { get; set; }

        public bool IsSelect { get; set; }

        public bool IsGroupBy { get; set; }

        public int Width { get; set; }
    }
    
}
