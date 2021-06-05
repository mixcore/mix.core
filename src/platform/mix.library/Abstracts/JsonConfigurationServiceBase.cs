using Mix.Shared.Constants;
using Mix.Shared.Services;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Threading;

namespace Mix.Lib.Abstracts
{
    public abstract class JsonConfigurationServiceBase
    {
        private static string filePath;
        protected static JObject AppSettings { get; set; }
        protected static string FilePath { get => filePath; set => filePath = value; }

        protected readonly FileSystemWatcher watcher = new();

        public JsonConfigurationServiceBase(string filePath)
        {
            FilePath = filePath;
            LoadAppSettings();
            WatchFile();
        }

        public static T GetConfig<T>(string name, T defaultValue = default)
        {
            var result = AppSettings[name];
            return result != null ? result.Value<T>() : defaultValue;
        }

        public static T GetConfig<T>(string culture, string name, T defaultValue = default)
        {
            JToken result = null;
            if (!string.IsNullOrEmpty(culture) && AppSettings[culture] != null)
            {
                result = AppSettings[culture][name];
            }
            return result != null ? result.Value<T>() : defaultValue;
        }

        public static T GetEnumConfig<T>(string name)
        {
            Enum.TryParse(typeof(T), AppSettings[name]?.Value<string>(), true, out object result);
            return result != null ? (T)result : default;
        }

        public static void SetConfig<T>(string name, T value)
        {
            AppSettings[name] = value != null ? JToken.FromObject(value) : null;
        }

        public static void SetConfig<T>(string culture, string name, T value)
        {
            AppSettings[culture][name] = value.ToString();
        }

        public static bool SaveSettings()
        {
            var settings = MixFileService.Instance.GetFile(FilePath, MixFileExtensions.Json, string.Empty, true, "{}");
            if (settings != null)
            {
                settings.Content = AppSettings.ToString();
                return MixFileService.Instance.SaveFile(settings);
            }
            else
            {
                return false;
            }
        }

        protected void WatchFile()
        {
            watcher.Path = Directory.GetCurrentDirectory();
            watcher.Filter = "";
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.EnableRaisingEvents = true;
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            Thread.Sleep(500);
            LoadAppSettings();
        }

        protected virtual void LoadAppSettings()
        {
            // Load configurations from appSettings.json
            var settings = MixFileService.Instance.GetFile(FilePath, MixFileExtensions.Json, string.Empty, true);

            string content = string.IsNullOrWhiteSpace(settings.Content) ? "{}" : settings.Content;
            JObject jsonSettings = JObject.Parse(content);
            AppSettings = jsonSettings;
        }
    }
}
