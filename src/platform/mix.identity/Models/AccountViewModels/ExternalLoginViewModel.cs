// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Mix.Identity.Constants;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Mix.Identity.Models.AccountViewModels
{
    public class ExternalLoginViewModel
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public string State { get; set; }

        public MixExternalLoginProviders Provider { get; set; }
    }

    public class RegisterExternalBindingModel
    {
        [Required]
        [JsonProperty("username")]
        public string UserName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [Required]
        [JsonProperty("provider")]
        public MixExternalLoginProviders Provider { get; set; }

        [Required]
        [JsonProperty("externalAccessToken")]
        public string ExternalAccessToken { get; set; }

    }

    public class ParsedExternalAccessToken
    {
        public string user_id { get; set; }
        public string app_id { get; set; }
    }
}