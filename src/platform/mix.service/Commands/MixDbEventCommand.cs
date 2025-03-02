using Mix.Heart.Helpers;
using Mix.Service.Models;
using Mix.Shared.Models;
using Newtonsoft.Json.Linq;

namespace Mix.Service.Commands
{
    public class MixDbEventCommand
    {
        public MixDbEventCommand(string createdBy, string action, string name, MixDbAuditLogModel data)
        {
            CreatedBy = createdBy;
            MixDbName = name;
            Action = action;
            Data = data;
        }

        public string CreatedBy { get; set; }
        public string MixDbName { get; set; }
        public string Action { get; set; }
        public MixDbAuditLogModel Data { get; set; }
    }
}
