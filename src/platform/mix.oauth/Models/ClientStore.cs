/*
                        GNU GENERAL PUBLIC LICENSE
                          Version 3, 29 June 2007
 Copyright (C) 2022 Mohammed Ahmed Hussien babiker Free Software Foundation, Inc. <https://fsf.org/>
 Everyone is permitted to copy and distribute verbatim copies
 of this license document, but changing it is not allowed.
 */

using System;
using System.Collections.Generic;

namespace Mix.OAuth.Models
{
    public class ClientStore
    {
        protected static readonly Lazy<ClientStore> _lazyInstance = new Lazy<ClientStore>(() => new ClientStore());

        /// <summary>
        /// Get the Singleton Instance for this Object.
        /// </summary>
        public static ClientStore Instance
        {
            get
            {
                return _lazyInstance.Value;
            }
        }

        public IEnumerable<Client> Clients = new[]
        {
            new Client
            {
                ClientName = "blazorWasm",
                ClientId = "1",
                ClientSecret = "123456789",
                AllowedScopes = new[]{ "openid", "profile", "blazorWasmapi.readandwrite" },
                GrantTypes = GrantTypes.Code,
                IsActive = false,
                ClientUri = "https://localhost:7026",
                RedirectUri = "https://localhost:7026/signin-oidc",
                UsePkce = true,
            },
            new Client
            {
                ClientName = "openIdtestapp",
                ClientId = "2",
                ClientSecret = "123456789",
                AllowedScopes = new[]{ "openid", "profile", "jwtapitestapp.read" },
                GrantTypes = GrantTypes.CodeAndClientCredentials,
                IsActive = true,
                ClientUri = "https://localhost:7276",
                RedirectUri = "https://localhost:7276/signin-oidc",
                UsePkce = true,
               // AllowedProtectedResources = new[]{ "jwtapitestapp" },
            },
              new Client
            {
                ClientName = "jwtapitestapp",
                ClientId = "3",
                ClientSecret = "123456789",
                AllowedScopes = new[]{ "jwtapitestapp.read", "jwtapitestapp.wite", "jwtapitestapp.readandwrite" },
                GrantTypes = GrantTypes.ClientCredentials,
                IsActive = true,
                ClientUri = "https://localhost:7065",
            }
        };
    }
}
