// Licensed to the mixcore Foundation under one or more agreements.
// The mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Mix.Cms.Lib.Models.Account;
using Mix.Cms.Lib.Services;
using Mix.Identity.Models;
using System;
using System.Text;

namespace Mix.Cms.Web
{
    //Ref: https://www.blinkingcaret.com/2017/09/06/secure-web-api-in-asp-net-core/
    public partial class Startup
    {
        protected void ConfigAuthorization(IServiceCollection services, IConfiguration Configuration)
        {
            ConfigIdentity(services, Configuration);
            ConfigJWTToken(services, Configuration);
            ConfigCookieAuth(services, Configuration);
        }

        private void ConfigIdentity(IServiceCollection services, IConfiguration Configuration)
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
        }

        protected void ConfigJWTToken(IServiceCollection services, IConfiguration Configuration)
        {
            services.AddAuthentication()
                    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.SaveToken = true;
                        options.TokenValidationParameters =
                             new TokenValidationParameters
                             {
                                 ClockSkew = TimeSpan.Zero,//.FromMinutes(MixService.GetAuthConfig<int>("ClockSkew")), //x minute tolerance for the expiration date
                                 ValidateIssuer = MixService.GetAuthConfig<bool>("ValidateIssuer"),
                                 ValidateAudience = MixService.GetAuthConfig<bool>("ValidateAudience"),
                                 ValidateLifetime = MixService.GetAuthConfig<bool>("ValidateLifetime"),
                                 ValidateIssuerSigningKey = MixService.GetAuthConfig<bool>("ValidateIssuerSigningKey"),
                                 //ValidIssuer = MixService.GetAuthConfig<string>("Issuer"),
                                 //ValidAudience = MixService.GetAuthConfig<string>("Audience"),
                                 ValidIssuers = MixService.GetAuthConfig<string>("Issuers").Split(','),
                                 ValidAudiences = MixService.GetAuthConfig<string>("Audiences").Split(','),
                                 IssuerSigningKey = JwtSecurityKey.Create(MixService.GetAuthConfig<string>("SecretKey"))
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
            services.AddAuthentication("Bearer");
        }

        protected void ConfigCookieAuth(IServiceCollection services, IConfiguration Configuration)
        {
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.MaxAge = TimeSpan.FromMinutes(MixService.GetAuthConfig<int>("CookieExpiration"));
                options.ExpireTimeSpan = TimeSpan.FromMinutes(MixService.GetAuthConfig<int>("CookieExpiration"));
                //options.Cookie.Expiration = TimeSpan.FromMinutes(MixService.GetAuthConfig<int>("CookieExpiration"));
                options.LoginPath = "/security/login"; // If the LoginPath is not set here, ASP.NET Core will default to /Account/Login
                options.LogoutPath = "/"; // If the LogoutPath is not set here, ASP.NET Core will default to /Account/Logout
                options.AccessDeniedPath = "/security/login"; // If the MixConstants.Default.DefaultCulture is not set here, ASP.NET Core will default to /Account/AccessDenied
                options.SlidingExpiration = true;
            });
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