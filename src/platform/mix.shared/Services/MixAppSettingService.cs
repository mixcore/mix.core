using Mix.Shared.Constants;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Threading;
using Mix.Shared.Enums;
using System.Collections.Generic;
using Mix.Shared.Models;
using Mix.Identity.Models;

namespace Mix.Shared.Services
{
    public class MixAppSettingService: SingletonService<MixAppSettingService>
    {
        public List<string> Cultures { get; set; }
        private static JObject AppSettings { get; set; }
        private static JObject DefaultAppSettings { get; set; }
        private static readonly FileSystemWatcher watcher = new();
        private static MixFileService _fileService;
        public MixAuthenticationConfigurations MixAuthentications 
        { 
            get => Instance.LoadSection<MixAuthenticationConfigurations>(MixAppSettingsSection.Authentication); 
        }
        public MixAppSettingService()
        {
            _fileService = new MixFileService();
            LoadAppSettings();
            LoadDefaultAppSettings();
        }
        public MixAppSettingService(MixFileService fileService)
        {
            _fileService = fileService;
            watcher.Path = System.IO.Directory.GetCurrentDirectory();
            watcher.Filter = "";
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.EnableRaisingEvents = true;
            LoadAppSettings();
            LoadDefaultAppSettings();
        }

        private static void LoadAppSettings()
        {
            // Load configurations from appSettings.json
            var settings = _fileService.GetFile(
                MixConstants.CONST_FILE_APPSETTING, MixFileExtensions.Json, string.Empty);

            if (string.IsNullOrEmpty(settings.Content))
            {
                settings = _fileService.GetFile(
                    MixConstants.CONST_DEFAULT_FILE_APPSETTING, MixFileExtensions.Json, string.Empty, true, "{}");
            }

            string content = string.IsNullOrWhiteSpace(settings.Content) ? "{}" : settings.Content;
            JObject jsonSettings = JObject.Parse(content);

            AppSettings = jsonSettings;

            // TODO: Init Mix.Heart Config
            //MixCommonHelper.WebConfigInstance = jsonSettings;
        }

        private static void LoadDefaultAppSettings()
        {
            // Load configurations from appSettings.json
            var settings = _fileService.GetFile(
                MixConstants.CONST_DEFAULT_FILE_APPSETTING, MixFileExtensions.Json, string.Empty, true);

            string content = string.IsNullOrWhiteSpace(settings.Content) ? "{}" : settings.Content;
            JObject jsonSettings = JObject.Parse(content);
            DefaultAppSettings = jsonSettings;
        }

        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            Thread.Sleep(500);
            LoadAppSettings();
        }

        public T LoadSection<T>(MixAppSettingsSection section)
        {
            return AppSettings[section.ToString()].ToObject<T>();
        }

        public T GetConfig<T>(MixAppSettingsSection section, string name, T defaultValue = default)
        {
            var result = AppSettings[section.ToString()][name];
            if (result == null)
            {
                result = DefaultAppSettings[section.ToString()][name];
            }
            return result != null ? result.Value<T>() : defaultValue;
        }

        public T GetEnumConfig<T>(MixAppSettingsSection section, string name)
        {
            Enum.TryParse(typeof(T), AppSettings[section.ToString()][name]?.Value<string>(), true, out object result);
            return result != null ? (T)result : default;
        }

        public void SetConfig<T>(MixAppSettingsSection section, string name, T value)
        {
            AppSettings[section.ToString()][name] = value != null ? JToken.FromObject(value) : null;
        }

        public JObject GetGlobalSetting()
        {
            return JObject.FromObject(AppSettings[MixAppSettingsSection.GlobalSettings.ToString()]);
        }

        public bool SaveSettings()
        {
            var settings = _fileService.GetFile(
                MixConstants.CONST_FILE_APPSETTING, MixFileExtensions.Json, string.Empty, true, "{}");
            if (settings != null)
            {
                if (string.IsNullOrWhiteSpace(settings.Content))
                {
                    var defaultSettings = _fileService.GetFile(
                        MixConstants.CONST_DEFAULT_FILE_APPSETTING, MixFileExtensions.Json, string.Empty, true, "{}");
                    settings = new FileViewModel()
                    {
                        Filename = "appsettings",
                        Extension = MixFileExtensions.Json,
                        Content = defaultSettings.Content
                    };
                    return _fileService.SaveFile(settings);
                }
                else
                {
                    JObject jsonSettings = JObject.Parse(settings.Content);

                    jsonSettings["ConnectionStrings"] = AppSettings[MixAppSettingsSection.ConnectionStrings.ToString()];
                    jsonSettings["GlobalSettings"] = AppSettings[MixAppSettingsSection.GlobalSettings.ToString()];
                    jsonSettings["GlobalSettings"]["LastUpdateConfiguration"] = DateTime.UtcNow;
                    jsonSettings["Authentication"] = AppSettings[MixAppSettingsSection.Authentication.ToString()];
                    jsonSettings["IpSecuritySettings"] = AppSettings[MixAppSettingsSection.IpSecuritySettings.ToString()];
                    jsonSettings["Smtp"] = AppSettings[MixAppSettingsSection.Smtp.ToString()];
                    jsonSettings["MixConfigurations"] = AppSettings[MixAppSettingsSection.MixConfigurations.ToString()];
                    settings.Content = jsonSettings.ToString();
                    return _fileService.SaveFile(settings);
                }
            }
            else
            {
                return false;
            }
        }

        public bool SaveSettings(string content)
        {
            var settings = _fileService.GetFile(
                MixConstants.CONST_FILE_APPSETTING, MixFileExtensions.Json, string.Empty, true, "{}");

            settings.Content = content;
            return _fileService.SaveFile(settings);
        }

        public void Reload()
        {
            LoadAppSettings();
            // TODO: Reload mix heart config
            //MixCommonHelper.ReloadWebConfig();
        }

        
    }
}