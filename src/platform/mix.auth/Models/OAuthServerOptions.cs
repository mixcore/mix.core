/*
                        GNU GENERAL PUBLIC LICENSE
                          Version 3, 29 June 2007
 Copyright (C) 2022 Mohammed Ahmed Hussien babiker Free Software Foundation, Inc. <https://fsf.org/>
 Everyone is permitted to copy and distribute verbatim copies
 of this license document, but changing it is not allowed.
 */

namespace Mix.Auth.Models
{
    public class OAuthServerOptions
    {
        /// <summary>
        /// This indicate which provider our identity provider will use
        /// InMemoey Or BackStore
        /// </summary>
        public string Provider { get; set; }

        /// <summary>
        /// If is not availabe so, no user can login or register
        /// This will shout down the application domain
        /// </summary>
        public bool IsAvaliable { get; set; }

        /// <summary>
        /// This is the uri of the identity provider
        /// </summary>
        public string IDPUri { get; set; }

    }
}
