// Licensed to the mixcore Foundation under one or more agreements.
// The mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Mix.Communicator.Services;
using Mix.Database.Entities.Account;
using Mix.Identity.Extensions;
using Mix.Lib.Services;
using System.Text;
namespace Microsoft.Extensions.DependencyInjection
{
    //Ref: https://www.blinkingcaret.com/2017/09/06/secure-web-api-in-asp-net-core/
    public static class AuthServiceCollectionExtensions
    {
        public static IServiceCollection AddMixAuthorize<TDbContext>(this IServiceCollection services)
            where TDbContext : DbContext
        {
            AuthConfigService authConfigService = services.GetService<AuthConfigService>();

            if (GlobalConfigService.Instance.IsInit)
            {
                authConfigService.AppSettings.SecretKey = Guid.NewGuid().ToString("N");
                authConfigService.SaveSettings();
            }

            var authConfigurations = authConfigService.AppSettings;
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
                options.User = new UserOptions() { RequireUniqueEmail = false };
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
                    // Sending the access token in the query string is required due to
                    // a limitation in Browser APIs. We restrict it to only calls to the
                    // SignalR hub in this code.
                    // See https://docs.microsoft.com/aspnet/core/signalr/security#access-token-logging
                    // for more information about security considerations when using
                    // the query string to transmit the access token.
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];

                            // If the request is for our hub...
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) &&
                                (path.StartsWithSegments("/hub")))
                            {
                                // Read the token out of the query string
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
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
            services.AddScoped<EmailService>();
            // Firebase service must be singleton (only one firebase default instance)
            services.TryAddSingleton<FirebaseService>();
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