﻿// Licensed to the mixcore Foundation under one or more agreements.
// The mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using Mix.Communicator.Services;
using Mix.Database.Entities.Account;
using Mix.Database.Entities.MixDb;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Identity.Extensions;
using Mix.Identity.Interfaces;
using Mix.Identity.Services;
using Mix.Lib.Extensions;
using Mix.Lib.Services;
using Mix.Shared.Models.Configurations;
using RabbitMQ.Client;
using System.Reflection;
using System.Text;
namespace Microsoft.Extensions.DependencyInjection
{
    //Ref: https://www.blinkingcaret.com/2017/09/06/secure-web-api-in-asp-net-core/
    public static class AuthServiceCollectionExtensions
    {
        public static IServiceCollection AddMixAuthorize<TDbContext>(this IServiceCollection services, IConfiguration configuration)
            where TDbContext : DbContext
        {
            var _globalConfig = configuration.Get<GlobalSettingsModel>()!;
            var authConfigService = services.GetService<AuthConfigService>();
            if (configuration.IsInit())
            {
                authConfigService.SetConfig(nameof(MixAuthenticationConfigurations.SecretKey), Guid.NewGuid().ToString("N"));
                authConfigService.SaveSettings();
            }
            services.AddMixIdentityConfigurations<TDbContext>(configuration);

            services.AddMixIdentityServices();
            return services;
        }

        public static IServiceCollection AddMixIdentityConfigurations<TDbContext>(this IServiceCollection services, IConfiguration configuration)
            where TDbContext : DbContext
        {
            var authConfigService = services.GetService<AuthConfigService>();
            var authConfigurations = authConfigService.AppSettings;
            PasswordOptions pOpt = new()
            {
                RequireDigit = false,
                RequiredLength = 4,
                RequireLowercase = false,
                RequireNonAlphanumeric = false,
                RequireUppercase = false
            };

            const string accessDeniedPath = "/security/login";

            services.AddIdentity<MixUser, MixRole>(options =>
            {
                options.Password = pOpt;
                options.User = new UserOptions()
                {
                    RequireUniqueEmail = authConfigurations.RequireUniqueEmail
                };
                //options.SignIn.RequireConfirmedEmail = authConfigurations.RequireConfirmedEmail;
            })
            .AddUserStore<TenantUserStore>()
            .AddRoleStore<TenantRoleStore>()
            .AddUserManager<TenantUserManager>()
            .AddRoleManager<TenantRoleManager>()
            .AddEntityFrameworkStores<TDbContext>()
            .AddDefaultTokenProviders();

            services.AddAuthorization();
            services.AddAuthentication(
                    opts =>
                    {
                        opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    }
                )
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
                    //options.ForwardDefaultSelector = ForwardReferenceToken("introspection");
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
                })
                 .AddMicrosoftIdentityWebApiIf(
                    !string.IsNullOrEmpty(authConfigurations.AzureAd?.ClientId),
                    configuration);
            
            services.AddRequiredScopeAuthorization();
            services.TryAddSingleton<IOAuthClientService, OAuthClientService>();
            services.TryAddSingleton<IOAuthCodeStoreService, OAuthCodeStoreService>();
            services.TryAddScoped<IOAuthTokenService, OAuthTokenService>();
            services.TryAddScoped<IOAuthTokenRevocationService, OAuthTokenRevocationService>();
            return services;
        }

        public static IServiceCollection AddMixIdentityServices(this IServiceCollection services)
        {
            services.TryAddSingleton<FirebaseService>();
            services.TryAddSingleton<FirestoreService>();
            services.TryAddScoped<MixDbDbContext>();
            services.TryAddScoped<UnitOfWorkInfo<MixDbDbContext>>();
            services.TryAddScoped<MixIdentityService>();
            return services;
        }

        public static IApplicationBuilder UseMixAuth(this IApplicationBuilder app)
        {
            //  If there are calls to app.UseRouting() and app.UseEndpoints(...), the call to app.UseAuthorization() must go between them.
            app.UseAuthentication();
            app.UseAuthorization();
            return app;
        }

        public static class JwtSecurityKey
        {
            public static SymmetricSecurityKey Create(string secret)
            {
                return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
            }
        }

        /// <summary>
        /// Provides a forwarding func for JWT vs reference tokens (based on existence of dot in token)
        /// </summary>
        /// <param name="introspectionScheme">Scheme name of the introspection handler</param>
        /// <returns></returns>
        public static Func<HttpContext, string> ForwardReferenceToken(string introspectionScheme = "introspection")
        {
            string Select(HttpContext context)
            {
                var (scheme, credential) = GetSchemeAndCredential(context);

                if (scheme.Equals("Bearer", StringComparison.OrdinalIgnoreCase) &&
                    !credential.Contains("."))
                {
                    return introspectionScheme;
                }

                return null;
            }

            return Select;
        }

        /// <summary>
        /// Extracts scheme and credential from Authorization header (if present)
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static (string, string) GetSchemeAndCredential(HttpContext context)
        {
            var header = context.Request.Headers["Authorization"].FirstOrDefault();

            if (string.IsNullOrEmpty(header))
            {
                return ("", "");
            }

            var parts = header.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2)
            {
                return ("", "");
            }

            return (parts[0], parts[1]);
        }
    }
}