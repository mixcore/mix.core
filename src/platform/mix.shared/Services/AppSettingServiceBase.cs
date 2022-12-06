using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace Mix.Shared.Services
{
    public abstract class AppSettingServiceBase<T>
    {
        private string _filePath;
        protected string _sectionName;
        public T AppSettings { get; set; }
        protected string FilePath { get => _filePath; set => _filePath = value; }

        protected readonly FileSystemWatcher watcher = new();
        protected readonly IConfiguration _configuration;

        public AppSettingServiceBase(IConfiguration configuration, string section, string filePath)
        {
            _configuration = configuration;
            _sectionName = section;
            FilePath = filePath;
            LoadAppSettings();
        }

        public bool SaveSettings()
        {
            var settings = MixFileHelper.GetFile(FilePath, MixFileExtensions.Json, string.Empty, true, "{}");
            if (settings != null)
            {
                settings.Content = new JObject(new JProperty(_sectionName, JObject.FromObject(AppSettings))).ToString();
                return !string.IsNullOrEmpty(MixFileHelper.SaveFile(settings));
            }
            else
            {
                return false;
            }
        }

        protected virtual void LoadAppSettings()
        {
            try
            {
                var settings = _configuration.GetSection(_sectionName);
                AppSettings = (T)Activator.CreateInstance(typeof(T));
                settings.Bind(AppSettings);
            }
            catch (Exception ex)
            {
                throw new MixException($"Cannot load config section {_sectionName}: {ex.Message}");
            }
        }
    }
}
