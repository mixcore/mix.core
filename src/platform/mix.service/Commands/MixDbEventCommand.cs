﻿using Newtonsoft.Json.Linq;

namespace Mix.Service.Commands
{
    public class MixDbEventCommand
    {
        public MixDbEventCommand(string createdBy, string action, string name, JObject data)
        {
            CreatedBy = createdBy;
            MixDbName = name;
            Action = action;
            Data = data;
        }

        public string CreatedBy { get; set; }
        public string MixDbName { get; set; }
        public string Action { get; set; }
        public JObject Data { get; set; }
    }
}
