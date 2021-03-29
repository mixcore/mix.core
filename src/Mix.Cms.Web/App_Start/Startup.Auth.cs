// Licensed to the mixcore Foundation under one or more agreements.
// The mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Models.Account;
using Mix.Cms.Lib.Services;
using Mix.Identity.Models;
using System;
using System.Text;

namespace Mix.Cms.Web
{
    //Ref: https://www.blinkingcaret.com/2017/09/06/secure-web-api-in-asp-net-core/
    public static class AuthServiceCollectionExtensions
    {
        public static IServiceCollection AddMixAuthorize(this IServiceCollection services, IConfiguration Configuration)
        {
            PasswordOptions pOpt = new PasswordOptions()
            {
                RequireDigit = false,
                RequiredLength = 6,
                RequireLowercase = false,
                RequireNonAlphanumeric = false,
                RequireUppercase = false
            };

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password = pOpt;
            })
                .AddEntityFrameworkStores<MixDbContext>()
                .AddDefaultTokenProviders()
                .AddUserManager<UserManager<ApplicationUser>>()

                ;

            services.AddAuthorization();

            services.AddAuthentication("Bearer")
                    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.SaveToken = true;
                        options.TokenValidationParameters =
                             new TokenValidationParameters
                             {
                                 ValidateIssuer = MixService.GetAuthConfig<bool>(MixAuthConfigurations.ValidateIssuer),
                                 ValidateAudience = MixService.GetAuthConfig<bool>(MixAuthConfigurations.ValidateAudience),
                                 ValidateLifetime = MixService.GetAuthConfig<bool>(MixAuthConfigurations.ValidateLifetime),
                                 ValidateIssuerSigningKey = MixService.GetAuthConfig<bool>(MixAuthConfigurations.ValidateIssuerSigningKey),
                                 ValidIssuers = MixService.GetAuthConfig<string>(MixAuthConfigurations.Issuers).Split(','),
                                 ValidAudiences = MixService.GetAuthConfig<string>(MixAuthConfigurations.Audiences).Split(','),
                                 IssuerSigningKey = JwtSecurityKey.Create(MixService.GetAuthConfig<string>(MixAuthConfigurations.SecretKey))
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
                options.Cookie.MaxAge = TimeSpan.FromMinutes(MixService.GetAuthConfig<int>(MixAuthConfigurations.CookieExpiration));
                options.ExpireTimeSpan = TimeSpan.FromMinutes(MixService.GetAuthConfig<int>(MixAuthConfigurations.CookieExpiration));
                options.LoginPath = "/security/login";
                options.LogoutPath = "/";
                options.AccessDeniedPath = "/security/login";
                options.SlidingExpiration = true;
            });
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