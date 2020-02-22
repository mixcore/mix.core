using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Mix.Identity.Models.AccountViewModels
{
    public class UserRoleModel
    {
        [JsonProperty("userId")]
        public string UserId { get; set; }
        [JsonProperty("roleId")]
        public string RoleId { get; set; }
        [JsonProperty("roleName")]
        public string RoleName { get; set; }
        [JsonProperty("isUserInRole")]
        public bool IsUserInRole { get; set; }
    }
}
