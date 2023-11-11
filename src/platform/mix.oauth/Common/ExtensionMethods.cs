/*
                        GNU GENERAL PUBLIC LICENSE
                          Version 3, 29 June 2007
 Copyright (C) 2022 Mohammed Ahmed Hussien babiker Free Software Foundation, Inc. <https://fsf.org/>
 Everyone is permitted to copy and distribute verbatim copies
 of this license document, but changing it is not allowed.
 */

using System.ComponentModel;
using System;
using System.Linq;

namespace Mix.OAuth.Common
{
    public static class ExtensionMethods
    {
        public static string? GetEnumDescription(this Enum en)
        {
            if (en == null) return null;

            var type = en.GetType();

            var memberInfo = type.GetMember(en.ToString());
            var description = (memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute),
                false).FirstOrDefault() as DescriptionAttribute)?.Description;

            return description;
        }



        public static bool IsRedirectUriStartWithHttps(this string redirectUri)
        {
            if (redirectUri != null && redirectUri.StartsWith("https")) return true;

            return false;
        }

        public static bool StringIsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }
    }
}
