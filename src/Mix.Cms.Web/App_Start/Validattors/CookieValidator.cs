using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mix.Cms.Web.Mvc.App_Start.Validattors
{
    public class CookieValidator
    {
        public static async Task ValidateAsync(CookieValidatePrincipalContext context)
        {
            // Do sth before process  request with current principal
            // context.RejectPrincipal();
            await Task.CompletedTask;
        }
    }
}
