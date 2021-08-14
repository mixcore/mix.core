// Licensed to the mixcore Foundation under one or more agreements.
// The mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using Mix.Identity.Extensions;
using Mix.Identity.Helpers;
using Mix.Shared.Models;
using Mix.Database.Entities.Account;
using Mix.Identity.Services;
using Mix.Shared.Enums;
using Mix.Shared.Services;

namespace Mix.Identity
{
    //Ref: https://www.blinkingcaret.com/2017/09/06/secure-web-api-in-asp-net-core/
    public static class AuthServiceCollectionExtensions
    {
        public static IServiceCollection AddMixAuthorize<TDbContext>(this IServiceCollection services)
            where TDbContext : DbContext
        {
            AuthConfigService authConfigService = new();
            var authConfigurations = authConfigService.AuthConfigurations;
            PasswordOptions pOpt = new PasswordOptions()
            {
                RequireDigit = false,
                RequiredLength = 6,
                RequireLowercase = false,
                RequireNonAlphanumeric = false,
                RequireUppercase = false
            };

            const string accessDeniedPath = "/security/login";

            services.AddIdentity<MixUser, IdentityRole>(options =>
            {
                options.Password = pOpt;
                options.User = new UserOptions() { RequireUniqueEmail = true };
            })
            .AddEntityFrameworkStores<TDbContext>()
            .AddDefaultTokenProviders()
            .AddUserManager<UserManager<MixUser>>();

            services.AddAuthorization();

            services.AddAuthentication(authConfigurations.TokenType)
                .AddFacebookIf(
                !string.IsNullOrEmpty(authConfigurations.Facebook?.AppId),
                authConfigurations.Facebook, accessDeniedPath)
                .AddGoogleIf(
                    !string.IsNullOrEmpty(authConfigurations.Google?.AppId),
                    authConfigurations.Google, accessDeniedPath)
                .AddTwitterIf(
                    !string.IsNullOrEmpty(authConfigurations.Twitter?.AppId),
                    authConfigurations.Twitter, accessDeniedPath)
                .AddMicrosoftAccountIf(
                    !string.IsNullOrEmpty(authConfigurations.Microsoft?.AppId),
                    authConfigurations.Microsoft, accessDeniedPath)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters =
                            new TokenValidationParameters
                            {
                                ClockSkew = TimeSpan.Zero,
                                ValidateIssuer = authConfigurations.ValidateIssuer,
                                ValidateAudience = authConfigurations.ValidateAudience,
                                ValidateLifetime = true,
                                ValidateIssuerSigningKey = authConfigurations.ValidateIssuerSigningKey,
                                ValidIssuers = authConfigurations.Issuers.Split(','),
                                ValidAudiences = authConfigurations.Audiences.Split(','),
                                IssuerSigningKey = JwtSecurityKey.Create(authConfigurations.SecretKey)
                            };
                    // TODO Handle Custom Auth
                    //options.Events = new JwtBearerEvents
                    //{
                    //    OnAuthenticationFailed = context =>
                    //    {
                    //        Console.WriteLine("OnAuthenticationFailed: " + context.Exception.Message);
                    //        return Task.CompletedTask;
                    //    },
                    //    OnTokenValidated = context =>
                    //    {
                    //        Console.WriteLine("OnTokenValidated: " + context.SecurityToken);
                    //        return Task.CompletedTask;
                    //    },
                    //};
                });

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.MaxAge = TimeSpan.FromMinutes(authConfigurations.AccessTokenExpiration);
                options.ExpireTimeSpan = TimeSpan.FromMinutes(authConfigurations.AccessTokenExpiration);
                options.LoginPath = accessDeniedPath;
                options.LogoutPath = "/";
                options.AccessDeniedPath = accessDeniedPath;
                options.SlidingExpiration = true;
            });
            services.AddScoped<MixIdentityHelper>();
            services.AddScoped<MixIdentityService>();
            return services;
        }

        public static IApplicationBuilder UseMixAuthorize(this IApplicationBuilder app)
        {
            return app;
        }

        protected static class JwtSecurityKey
        {
            public static SymmetricSecurityKey Create(string secret)
            {
                return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
            }
        }
    }
}