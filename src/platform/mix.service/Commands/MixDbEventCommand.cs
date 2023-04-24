using Mix.Heart.Helpers;
using Mix.Service.Models;
using Mix.Shared.Models;
using Newtonsoft.Json.Linq;

namespace Mix.Service.Commands
{
    public class MixDbEventCommand
    {
        public MixDbEventCommand(string action, string name, JObject data)
        {
            MixDbName = name;
            Action = action;
            Data = data;
        }

        public string MixDbName { get; set; }
        public string Action { get; set; }
        public JObject Data { get; set; }
    }
}
