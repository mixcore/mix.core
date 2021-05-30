using Mix.Infrastructure.Repositories;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Threading;

namespace Mix.Lib.Abstracts
{
    public abstract class JsonConfigurationServiceBase
    {
        protected static string _filePath;
        protected static JObject AppSettings { get; set; }
        protected readonly FileSystemWatcher watcher = new FileSystemWatcher();

        public JsonConfigurationServiceBase(string filePath)
        {
            _filePath = filePath;
            LoadAppSettings();
            WatchFile();
        }

        public T GetConfig<T>(string name, T defaultValue = default)
        {
            var result = AppSettings[name];
            return result != null ? result.Value<T>() : defaultValue;
        }

        public T GetConfig<T>(string culture, string name, T defaultValue = default)
        {
            JToken result = null;
            if (!string.IsNullOrEmpty(culture) && AppSettings[culture] != null)
            {
                result = AppSettings[culture][name];
            }
            return result != null ? result.Value<T>() : defaultValue;
        }

        public T GetEnumConfig<T>(string name)
        {
            Enum.TryParse(typeof(T), AppSettings[name]?.Value<string>(), true, out object result);
            return result != null ? (T)result : default;
        }

        public void SetConfig<T>(string name, T value)
        {
            AppSettings[name] = value != null ? JToken.FromObject(value) : null;
        }

        public void SetConfig<T>(string culture, string name, T value)
        {
            AppSettings[culture][name] = value.ToString();
        }

        public bool SaveSettings()
        {
            var settings = MixFileRepository.Instance.GetFile(_filePath, string.Empty, true, "{}");
            if (settings != null)
            {
                settings.Content = AppSettings.ToString();
                return MixFileRepository.Instance.SaveFile(settings);
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
            JObject jsonSettings = new JObject();
            var settings = MixFileRepository.Instance.GetFile(_filePath, string.Empty, true);

            string content = string.IsNullOrWhiteSpace(settings.Content) ? "{}" : settings.Content;
            jsonSettings = JObject.Parse(content);

            AppSettings = jsonSettings;
        }
    }
}
