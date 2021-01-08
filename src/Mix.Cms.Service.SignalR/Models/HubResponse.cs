﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Constants;
namespace Mix.Cms.Service.SignalR.Models
{
    public class HubResponse<T>
    {
        [JsonProperty("isSucceed")]
        public bool IsSucceed { get; set; }

        [JsonProperty("responseKey")]
        public string ResponseKey { get; set; }

        [JsonProperty("errors")]
        public List<string> Errors { get; set; } = new List<string>();

        [JsonProperty("exception")]
        public Exception Exception { get; set; }

        [JsonProperty("data")]
        public T Data { get; set; }

        [JsonProperty("renderDate")]
        public DateTime RenderDate { get { return DateTime.UtcNow; } }
    }
}