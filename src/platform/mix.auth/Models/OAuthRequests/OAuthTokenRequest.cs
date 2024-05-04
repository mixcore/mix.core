/*
                        GNU GENERAL PUBLIC LICENSE
                          Version 3, 29 June 2007
 Copyright (C) 2022 Mohammed Ahmed Hussien babiker Free Software Foundation, Inc. <https://fsf.org/>
 Everyone is permitted to copy and distribute verbatim copies
 of this license document, but changing it is not allowed.
 */

namespace Mix.Auth.Models.OAuthRequests
{
    public class OAuthTokenRequest
    {
        public Guid client_id { get; set; }
        public string client_secret { get; set; }
        public string code { get; set; }
        public string grant_type { get; set; }
        public string redirect_uri { get; set; }
        public string code_verifier { get; set; }
        public IList<string> scope { get; set; }
    }
}
