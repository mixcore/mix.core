using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Mix.Identity.Identity.Models.AccountViewModels
{

    public class CreateRoleBindingModel
    {

        [Required]
        [StringLength(256, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [Display(Name = "Role Name")]
        [JsonProperty("name")]
        public string Name { get; set; }

    }
}
