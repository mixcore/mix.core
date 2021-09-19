using Newtonsoft.Json.Linq;

namespace Mix.Cms.Lib.Models.Common
{
    public class EdmInfoModel
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string From { get; set; }
        public string[] Recipients { get; set; }
        public string[] CCs { get; set; }
        public JObject Data { get; set; }
    }
}
