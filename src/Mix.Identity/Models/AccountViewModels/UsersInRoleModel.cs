using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mix.Identity.Identity.Models.AccountViewModels
{
    public class UsersInRoleModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("EnrolledUsers")]
        public List<string> EnrolledUsers { get; set; }
        [JsonProperty("removedUsers")]
        public List<string> RemovedUsers { get; set; }
    }
}
