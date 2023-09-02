// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Mix.Identity.Constants;

namespace Mix.Identity.Models.AccountViewModels
{
    public class ExternalLoginResultModel
    {
        public string Token { get; set; }
        public string ReturnUrl { get; set; }
    }
    public class ExternalLoginViewModel
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public string State { get; set; }

        public MixExternalLoginProviders Provider { get; set; }
    }

    public class RegisterExternalBindingModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public MixExternalLoginProviders Provider { get; set; }

        public string ExternalAccessToken { get; set; }

        //public JObject Data { get; set; }

    }

    public class ParsedExternalAccessToken
    {
        public string user_id { get; set; }
        public string app_id { get; set; }
    }
}