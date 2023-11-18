/*
                        GNU GENERAL PUBLIC LICENSE
                          Version 3, 29 June 2007
 Copyright (C) 2022 Mohammed Ahmed Hussien babiker Free Software Foundation, Inc. <https://fsf.org/>
 Everyone is permitted to copy and distribute verbatim copies
 of this license document, but changing it is not allowed.
 */

using Mix.OAuth.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace Mix.OAuth.Services.CodeServce
{
    public interface ICodeStoreService
    {
        string? GenerateAuthorizationCode(AuthorizationCode authorizationCode);
        AuthorizationCode? GetClientDataByCode(string key);
        AuthorizationCode? UpdatedClientDataByCode(string key, ClaimsPrincipal claimsPrincipal, IList<string> requestdScopes);
        AuthorizationCode? RemoveClientDataByCode(string key);
    }
}
