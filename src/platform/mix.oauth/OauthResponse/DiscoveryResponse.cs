/*
                        GNU GENERAL PUBLIC LICENSE
                          Version 3, 29 June 2007
 Copyright (C) 2022 Mohammed Ahmed Hussien babiker Free Software Foundation, Inc. <https://fsf.org/>
 Everyone is permitted to copy and distribute verbatim copies
 of this license document, but changing it is not allowed.
 */

using System.Collections.Generic;

namespace Mix.OAuth.OauthResponse
{
    public class DiscoveryResponse
    {
        public string issuer { get; set; }
        public string authorization_endpoint { get; set; }
        public string token_endpoint { get; set; }
        public IList<string> token_endpoint_auth_methods_supported { get; set; }
        public IList<string> token_endpoint_auth_signing_alg_values_supported { get; set; }
        public string userinfo_endpoint { get; set; }
        public string check_session_iframe { get; set; }
        public string end_session_endpoint { get; set; }
        public string jwks_uri { get; set; }
        public string registration_endpoint { get; set; }
        public IList<string> scopes_supported { get; set; }
        public IList<string> response_types_supported { get; set; }
        public IList<string> acr_values_supported { get; set; }
        public IList<string> subject_types_supported { get; set; }
        public IList<string> userinfo_signing_alg_values_supported { get; set; }
        public IList<string> userinfo_encryption_alg_values_supported { get; set; }
        public IList<string> userinfo_encryption_enc_values_supported { get; set; }
        public IList<string> id_token_signing_alg_values_supported { get; set; }
        public IList<string> id_token_encryption_alg_values_supported { get; set; }
        public IList<string> id_token_encryption_enc_values_supported { get; set; }
        public IList<string> request_object_signing_alg_values_supported { get; set; }
        public IList<string> display_values_supported { get; set; }
        public IList<string> claim_types_supported { get; set; }
        public IList<string> claims_supported { get; set; }
        public bool claims_parameter_supported { get; set; }
        public string service_documentation { get; set; }
        public IList<string> ui_locales_supported { get; set; }
        public string introspection_endpoint { get; set; }
    }
}
