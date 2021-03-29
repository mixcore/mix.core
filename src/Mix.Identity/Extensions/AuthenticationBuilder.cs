using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Mix.Identity.Models;

namespace Mix.Identity.Extensions
{
    public static class AuthenticationBuilderExtensions
    {
        public static AuthenticationBuilder AddFacebookIf(
            this AuthenticationBuilder builder, 
            bool condition, 
            ExternalLogin config,
            string accessDeniedPath)
        {
            return condition ? builder.AddFacebook(options =>
            {
                if (!string.IsNullOrEmpty(config.AppId))
                {
                    options.AppId = config.AppId;
                    options.AppSecret = config.AppSecret;
                    options.AccessDeniedPath = accessDeniedPath;
                }
            }) : builder;
        }
        
        public static AuthenticationBuilder AddGoogleIf(
            this AuthenticationBuilder builder, 
            bool condition, 
            ExternalLogin config,
            string accessDeniedPath)
        {
            return condition ? builder.AddGoogle(options =>
            {
                if (!string.IsNullOrEmpty(config.AppId))
                {
                    options.ClientId = config.AppId;
                    options.ClientSecret = config.AppSecret;
                    options.AccessDeniedPath = accessDeniedPath;
                }
            }) : builder;
        }
        
        public static AuthenticationBuilder AddTwitterIf(
            this AuthenticationBuilder builder, 
            bool condition, 
            ExternalLogin config,
            string accessDeniedPath)
        {
            return condition ? builder.AddTwitter(options =>
            {
                if (!string.IsNullOrEmpty(config.AppId))
                {
                    options.ConsumerKey = config.AppId;
                    options.ConsumerSecret = config.AppSecret;
                    options.RetrieveUserDetails = true;
                    options.AccessDeniedPath = accessDeniedPath;
                }
            }) : builder;
        }

        public static AuthenticationBuilder AddMicrosoftAccountIf(
           this AuthenticationBuilder builder,
           bool condition,
           ExternalLogin config,
           string accessDeniedPath)
        {
            return condition ? builder.AddMicrosoftAccount(options =>
            {
                if (!string.IsNullOrEmpty(config.AppId))
                {
                    options.ClientId = config.AppId;
                    options.ClientSecret = config.AppSecret;
                    options.AccessDeniedPath = accessDeniedPath;
                }
            }) : builder;
        }

    }
}
