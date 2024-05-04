﻿// Licensed to the mixcore Foundation under one or more agreements.
// The mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Mix.Communicator.Services;
using Mix.Database.Entities.Account;
using Mix.Database.Entities.MixDb;
using Mix.Identity.Extensions;
using Mix.Identity.Interfaces;
using Mix.Identity.Services;
using Mix.Lib.Services;
using Mix.Shared.Models.Configurations;
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
            if (_globalConfig.IsInit)
            {
                authConfigService.AppSettings.SecretKey = Guid.NewGuid().ToString("N");
                authConfigService.SaveSettings();
            }
            services.AddMixIdentityConfigurations<TDbContext>(configuration);

            services.AddMixIdentityServices();
            return services;
        }

        public static IServiceCollection AddMixIdentityConfigurations<TDbContext>(this IServiceCollection services, IConfiguration configuration)
            where TDbContext : DbContext
        {


            var authConfigurations = configuration.GetSection(MixAppSettingsSection.Authentication).Get<MixAuthenticationConfigurations>();
            PasswordOptions pOpt = new()
            {
                RequireDigit = false,
                RequiredLength = 6,
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
            //services.ConfigureApplicationCookie(options =>
            //{
            //    options.Cookie.Name = authConfigurations.Issuer;
            //    options.Cookie.HttpOnly = true;
            //    options.Cookie.MaxAge = TimeSpan.FromMinutes(authConfigurations.AccessTokenExpiration);
            //    options.ExpireTimeSpan = TimeSpan.FromMinutes(authConfigurations.AccessTokenExpiration);
            //    options.LoginPath = accessDeniedPath;
            //    options.LogoutPath = "/";
            //    options.AccessDeniedPath = accessDeniedPath;
            //    options.SlidingExpiration = true;
            //});
            // Firebase service must be singleton (only one firebase default instance)

            services.TryAddSingleton<IOAuthClientService, OAuthClientService>();
            services.TryAddSingleton<IOAuthCodeStoreService, OAuthCodeStoreService>();
            services.TryAddScoped<IOAuthTokenService, OAuthTokenService>();
            services.TryAddScoped<IOAuthTokenRevocationService, OAuthTokenRevocationService>();
            return services;
        }

        public static IServiceCollection AddMixIdentityServices(this IServiceCollection services)
        {
            services.TryAddSingleton<FirebaseService>();
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
    }
}