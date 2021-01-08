﻿using Microsoft.AspNetCore.Authentication.Cookies;
using System.Threading.Tasks;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Constants;
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