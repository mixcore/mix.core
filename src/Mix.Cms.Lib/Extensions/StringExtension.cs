using System;

namespace Mix.Cms.Lib.Extensions
{
    public static class StringExtension
    {
        public static string ToCamelCase(this string str)
        {
            if (!string.IsNullOrEmpty(str) && str.Length > 1)
            {
                return Char.ToLowerInvariant(str[0]) + str.Substring(1);
            }
            return str;
        }

        public static string ToTitleCase(this string str)
        {
            if (!string.IsNullOrEmpty(str) && str.Length > 1)
            {
                return Char.ToUpperInvariant(str[0]) + str.Substring(1);
            }
            return str;
        }

        public static bool IsBase64(this string base64String)
        {
            base64String = base64String.IndexOf(',') >= 0 ? base64String.Split(',')[1] : base64String;
            if (string.IsNullOrEmpty(base64String) || base64String.Length % 4 != 0
               || base64String.Contains(" ") || base64String.Contains("\t") || base64String.Contains("\r") || base64String.Contains("\n"))
                return false;

            try
            {
                Convert.FromBase64String(base64String);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}