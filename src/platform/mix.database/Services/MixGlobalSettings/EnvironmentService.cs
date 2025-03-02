using Microsoft.Extensions.Configuration;
using Mix.Database.Entities.Settings;
using Mix.Heart.Extensions;
using Mix.Heart.Helpers;
using Mix.Shared.Models.Configurations;
using System.ComponentModel;
using System.Linq;

namespace Mix.Database.Services.MixGlobalSettings
{
    public class EnvironmentService
    {
        public static string ServiceName
        {
            get => GetEnvironmentVariable<string>(MixConstants.EnvironmentKeys.SERVICE_NAME) ?? "portal";
        }
        
        public static T GetEnvironmentVariable<T>(string key)
        {
            return GetValue<T>(Environment.GetEnvironmentVariable(key));
        }

        public static void SetEnvironmentVariable<T>(string key, T value)
        {
            Environment.SetEnvironmentVariable( key, value.ToString(), EnvironmentVariableTarget.Process);
        }

        private static T GetValue<T>(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return default;
            }
            return Parse<T>(content);
        }

        public static object Parse(Type t, string s)
        => TypeDescriptor.GetConverter(t).ConvertFromInvariantString(s);

        public static T Parse<T>(string s)
           => (T)Parse(typeof(T), s);
    }
}
