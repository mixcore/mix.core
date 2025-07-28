// Licensed to the mixcore Foundation under one or more agreements.
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
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    // Ref: https://www.blinkingcaret.com/2017/09/06/secure-web-api-in-asp-net-core/
    public static class AuthServiceCollectionExtensions
    {
        public static IServiceCollection AddMixAuthorize<TDbContext>(this IServiceCollection services, IConfiguration configuration)
            where TDbContext : DbContext
        {
            services.AddMixIdentityConfigurations<TDbContext>(configuration);
            services.AddMixIdentityServices();
            return services;
        }

        public static IServiceCollection AddMixIdentityConfigurations<TDbContext>(this IServiceCollection services, IConfiguration configuration)
            where TDbContext : DbContext
        {
            var authConfigService = services.GetService<AuthConfigService>();
            if (string.IsNullOrEmpty(authConfigService.AppSettings.SecretKey))
            {
                authConfigService.SetConfig(nameof(MixAuthenticationConfigurations.SecretKey), Guid.NewGuid().ToString("N"), true);
            }
            var authConfigurations = authConfigService.AppSettings;
            var passwordOptions = new PasswordOptions
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
                options.Password = passwordOptions;
                options.User = new UserOptions
                {
                    RequireUniqueEmail = authConfigurations.RequireUniqueEmail
                };
            })
            .AddUserStore<TenantUserStore>()
            .AddRoleStore<TenantRoleStore>()
            .AddUserManager<TenantUserManager>()
            .AddRoleManager<TenantRoleManager>()
            .AddEntityFrameworkStores<TDbContext>()
            .AddDefaultTokenProviders();

            services.AddAuthorization();
            services.AddAuthentication(opts =>
            {
                opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddFacebookIf(!string.IsNullOrEmpty(authConfigurations.Facebook?.AppId), authConfigurations.Facebook, accessDeniedPath)
            .AddGoogleIf(!string.IsNullOrEmpty(authConfigurations.Google?.AppId), authConfigurations.Google, accessDeniedPath)
            .AddTwitterIf(!string.IsNullOrEmpty(authConfigurations.Twitter?.AppId), authConfigurations.Twitter, accessDeniedPath)
            .AddMicrosoftAccountIf(!string.IsNullOrEmpty(authConfigurations.Microsoft?.AppId), authConfigurations.Microsoft, accessDeniedPath)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
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
            .AddMicrosoftIdentityWebApiIf(!string.IsNullOrEmpty(authConfigurations.AzureAd?.ClientId), configuration);

            services.AddSession(
                options =>
                {
                    options.IdleTimeout = TimeSpan.FromMinutes(authConfigService.AppSettings.AccessTokenExpiration);
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.Cookie.SameSite = SameSiteMode.Strict;
                    options.Cookie.HttpOnly = true;
                    options.Cookie.Name = authConfigService.AppSettings.Issuer;
                }
            );

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
            services.TryAddScoped<MixPermissionService>();
            return services;
        }

        public static IApplicationBuilder UseMixAuth(this IApplicationBuilder app)
        {
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

        public static Func<HttpContext, string> ForwardReferenceToken(string introspectionScheme = "introspection")
        {
            string Select(HttpContext context)
            {
                var (scheme, credential) = GetSchemeAndCredential(context);
                if (scheme.Equals("Bearer", StringComparison.OrdinalIgnoreCase) && !credential.Contains("."))
                {
                    return introspectionScheme;
                }
                return null;
            }
            return Select;
        }

        public static (string, string) GetSchemeAndCredential(HttpContext context)
        {
            var header = context.Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrEmpty(header))
            {
                return (string.Empty, string.Empty);
            }
            var parts = header.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2)
            {
                return (string.Empty, string.Empty);
            }
            return (parts[0], parts[1]);
        }
    }
}