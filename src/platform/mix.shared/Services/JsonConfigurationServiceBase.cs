using Newtonsoft.Json.Linq;

namespace Mix.Shared.Services
{
    public class JsonConfigurationServiceBase
    {
        public JObject AppSettings { get; set; }
        protected string FilePath { get; set; }

        protected readonly FileSystemWatcher watcher = new();

        public JsonConfigurationServiceBase(string filePath)
        {
            FilePath = filePath;
            LoadAppSettings();
            //WatchFile();
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
            AppSettings[name] = object.Equals(value, default(T)) ? JToken.FromObject(value) : null;
            SaveSettings();
        }

        public void SetConfig<T>(string culture, string name, T value)
        {
            AppSettings[culture][name] = value.ToString();
            SaveSettings();
        }

        public bool SaveSettings()
        {
            var settings = MixFileHelper.GetFileByFullName($"{FilePath}{MixFileExtensions.Json}", true);
            if (settings != null)
            {
                settings.Content = AppSettings.ToString();
                return MixFileHelper.SaveFile(settings);
            }
            else
            {
                return false;
            }
        }
        protected virtual void LoadAppSettings()
        {
            var settings = MixFileHelper.GetFileByFullName($"{FilePath}{MixFileExtensions.Json}", true);
            string content = string.IsNullOrWhiteSpace(settings.Content) ? "{}" : settings.Content;
            JObject jsonSettings = JObject.Parse(content);
            AppSettings = jsonSettings;
        }
    }
}
