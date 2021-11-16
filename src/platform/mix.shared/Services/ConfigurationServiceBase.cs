using Mix.Heart.Services;
using Mix.Shared.Constants;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Threading;

namespace Mix.Shared.Services
{
    public class ConfigurationServiceBase<T>
    {
        private string filePath;
        private JObject _obj;
        public T AppSettings { get; set; }
        protected string FilePath { get => filePath; set => filePath = value; }

        protected readonly FileSystemWatcher watcher = new();

        public ConfigurationServiceBase(string filePath)
        {
            FilePath = filePath;
            LoadAppSettings();
            WatchFile();
        }

        public T GetConfig<T>(string name, T defaultValue = default)
        {
            var result = _obj[name];
            return result != null ? result.Value<T>() : defaultValue;
        }

        public T GetConfig<T>(string culture, string name, T defaultValue = default)
        {
            JToken result = null;
            if (!string.IsNullOrEmpty(culture) && _obj[culture] != null)
            {
                result = _obj[culture][name];
            }
            return result != null ? result.Value<T>() : defaultValue;
        }

        public T GetEnumConfig<T>(string name)
        {
            Enum.TryParse(typeof(T), _obj[name]?.Value<string>(), true, out object result);
            return result != null ? (T)result : default;
        }

        public void SetConfig<T>(string name, T value)
        {
            _obj[name] = value != null ? JToken.FromObject(value) : null;
            SaveSettings();
        }

        public void SetConfig<T>(string culture, string name, T value)
        {
            _obj[culture][name] = value.ToString();
            SaveSettings();
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
            _obj = JObject.Parse(content);
            AppSettings = _obj.ToObject<T>();
        }
    }
}
