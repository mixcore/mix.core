using Microsoft.Extensions.Configuration;
using Mix.Heart.Extensions;
using Mix.Heart.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mix.Shared.Services
{
    public abstract class AppSettingServiceBase<T>
    {
        private string _filePath;
        public JObject RawSettings;
        protected string _sectionName;
        protected bool _isEncrypt;
        protected string _aesKey { get; set; }
        public T AppSettings { get; set; }
        protected string FilePath { get => _filePath; set => _filePath = value; }

        protected readonly FileSystemWatcher watcher = new();
        protected readonly IConfiguration _configuration;

        public AppSettingServiceBase(IConfiguration configuration, string section, string filePath, bool isEncrypt)
        {
            _configuration = configuration;
            _sectionName = section;
            _isEncrypt = isEncrypt;
            _aesKey = GlobalConfigService.Instance.AesKey;
            FilePath = filePath;
            LoadAppSettings();
        }

        public void SetConfig<TValue>(string name, TValue value)
        {
            RawSettings[name] = value != null ? JToken.FromObject(value) : null;
            AppSettings = RawSettings.ToObject<T>();
        }

        public virtual bool SaveSettings()
        {
            var settings = MixFileHelper.GetFileByFullName($"{FilePath}{MixFileExtensions.Json}", true, "{}");
            if (settings != null)
            {
                settings.Content = string.IsNullOrEmpty(_sectionName)
                                    ? RawSettings.ToString(Formatting.None)
                                    : ReflectionHelper.ParseObject(
                                        new JObject(
                                            new JProperty(_sectionName, RawSettings)))
                                    .ToString(Formatting.None);
                if (_isEncrypt)
                {
                    settings.Content = AesEncryptionHelper.EncryptString(settings.Content, _aesKey);
                }
                var result = MixFileHelper.SaveFile(settings);
                return result;
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
                var settings = MixFileHelper.GetFileByFullName($"{FilePath}{MixFileExtensions.Json}", true, "{}");
                string content = string.IsNullOrWhiteSpace(settings.Content) ? "{}" : settings.Content;
                bool isContentEncrypted = !content.IsJsonString();


                if (isContentEncrypted)
                {
                    content = AesEncryptionHelper.DecryptString(content, _aesKey);
                }

                var rawSettings = JObject.Parse(content);
                RawSettings = !string.IsNullOrEmpty(_sectionName)
                    ? rawSettings[_sectionName] as JObject
                    : rawSettings;
                AppSettings = RawSettings.ToObject<T>();

                if (_isEncrypt && !isContentEncrypted)
                {
                    SaveSettings();
                }
            }
            catch (Exception ex)
            {
                throw new MixException($"Cannot load config section {_sectionName}: {ex.Message}");
            }
        }
    }
}
