using Mix.Shared.Constants;
using Mix.Shared.Services;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Threading;

namespace Mix.Shared.Abstracts
{
    public abstract class JsonConfigurationServiceBase
    {
        private string filePath;
        protected JObject AppSettings { get; set; }
        protected string FilePath { get => filePath; set => filePath = value; }

        protected readonly FileSystemWatcher watcher = new();

        public JsonConfigurationServiceBase(string filePath)
        {
            FilePath = filePath;
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
            watcher.Path = FilePath[..FilePath.LastIndexOf('/')];
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
            var settings = MixFileService.Instance.GetFile(FilePath, MixFileExtensions.Json, string.Empty, true);
            string content = string.IsNullOrWhiteSpace(settings.Content) ? "{}" : settings.Content;
            JObject jsonSettings = JObject.Parse(content);
            AppSettings = jsonSettings;
        }
    }
}
