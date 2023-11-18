/*
                        GNU GENERAL PUBLIC LICENSE
                          Version 3, 29 June 2007
 Copyright (C) 2022 Mohammed Ahmed Hussien babiker Free Software Foundation, Inc. <https://fsf.org/>
 Everyone is permitted to copy and distribute verbatim copies
 of this license document, but changing it is not allowed.
 */

using Microsoft.AspNetCore.Http;
using Mix.OAuth.OauthResponse;
using System.Threading.Tasks;

namespace Mix.OAuth.Services
{
    public interface ITokenRevocationService
    {
        Task<TokenRecovationResponse> RevokeTokenAsync(HttpContext httpContext, string clientId);
    }
}
