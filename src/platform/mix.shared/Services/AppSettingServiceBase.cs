using Microsoft.Extensions.Configuration;
using Mix.Heart.Extensions;
using Mix.Heart.Helpers;
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
            _aesKey = GlobalConfigService.Instance.AppSettings.ApiEncryptKey;
            FilePath = filePath;
            LoadAppSettings();
        }

        public bool SaveSettings()
        {
            var settings = MixFileHelper.GetFile(FilePath, MixFileExtensions.Json, string.Empty, true, "{}");
            if (settings != null)
            {

                if (_isEncrypt && settings.Content.IsJsonString())
                {
                    settings.Content = AesEncryptionHelper.EncryptString(settings.Content, _aesKey);
                }
                else
                {
                    settings.Content = new JObject(new JProperty(_sectionName, JObject.FromObject(AppSettings))).ToString(Newtonsoft.Json.Formatting.None);
                }
                return MixFileHelper.SaveFile(settings);
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
                if (_isEncrypt)
                {
                    if (!content.IsJsonString())
                    {
                        content = AesEncryptionHelper.DecryptString(content, _aesKey);
                    }
                    else
                    {
                        // Encrypt and save to setting file if not encrypted
                        settings.Content = AesEncryptionHelper.EncryptString(content, _aesKey);
                        MixFileHelper.SaveFile(settings);
                    }
                }
                RawSettings = JObject.Parse(content);
                AppSettings = string.IsNullOrEmpty(_sectionName) ? RawSettings.ToObject<T>()
               : RawSettings[_sectionName].ToObject<T>();
            }
            catch (Exception ex)
            {
                throw new MixException($"Cannot load config section {_sectionName}: {ex.Message}");
            }
        }
    }
}
