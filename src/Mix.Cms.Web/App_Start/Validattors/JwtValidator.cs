using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mix.Cms.Web.Mvc.App_Start.Validattors
{
    public class JwtValidator
    {
        public static void ValidateAsync(TokenValidatedContext context)
        {
            // Do sth before process  request with current principal
            // context.RejectPrincipal();
            Console.WriteLine("OnTokenValidated: " + context.SecurityToken);

        }

        public static void ValidateFail(AuthenticationFailedContext context)
        {
            // Do sth when validate failed request with current principal
            // context.RejectPrincipal();
            Console.WriteLine("OnAuthenticationFailed: " + context.Exception.Message);

        }
    }
}
