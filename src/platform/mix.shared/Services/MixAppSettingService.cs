using Mix.Shared.Constants;
using Mix.Common.Helper;
using Mix.Heart.Models;
using Mix.Infrastructure.Repositories;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Threading;
using Mix.Identity.Models;
using Mix.Shared.Enums;
using System.Collections.Generic;

namespace Mix.Shared.Services
{
    public class MixAppSettingService
    {
        /// <summary>
        /// The synchronize root
        /// </summary>
        private static readonly object syncRoot = new();

        /// <summary>
        /// The instance
        /// </summary>
        private static volatile MixAppSettingService instance;

        private static volatile MixAppSettingService defaultInstance;

        public List<string> Cultures { get; set; }
        private JObject AppSettings { get; set; }
        private readonly FileSystemWatcher watcher = new();
        public static MixAuthenticationConfigurations MixAuthentications { get => Instance.AppSettings[MixAppSettingsSection.Authentication.ToString()].ToObject<MixAuthenticationConfigurations>(); }

        public MixAppSettingService()
        {
            watcher.Path = System.IO.Directory.GetCurrentDirectory();
            watcher.Filter = "";
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.EnableRaisingEvents = true;
        }

        public static MixAppSettingService Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new MixAppSettingService();
                            LoadAppSettings();
                        }
                    }
                }

                return instance;
            }
        }

        public static MixAppSettingService DefaultInstance
        {
            get
            {
                if (defaultInstance == null)
                {
                    lock (syncRoot)
                    {
                        if (defaultInstance == null)
                        {
                            defaultInstance = new MixAppSettingService();
                            LoadDefaultAppSettings();
                        }
                    }
                }

                return defaultInstance;
            }
        }

        private static void LoadAppSettings()
        {
            // Load configurations from appSettings.json
            var settings = MixFileRepository.Instance.GetFile(MixConstants.CONST_FILE_APPSETTING, string.Empty, true);

            if (string.IsNullOrEmpty(settings.Content))
            {
                settings = MixFileRepository.Instance.GetFile(MixConstants.CONST_DEFAULT_FILE_APPSETTING, string.Empty, true, "{}");
            }

            string content = string.IsNullOrWhiteSpace(settings.Content) ? "{}" : settings.Content;
            JObject jsonSettings = JObject.Parse(content);

            instance.AppSettings = jsonSettings;
            MixCommonHelper.WebConfigInstance = jsonSettings;
        }

        private static void LoadDefaultAppSettings()
        {
            // Load configurations from appSettings.json
            var settings = MixFileRepository.Instance.GetFile(MixConstants.CONST_DEFAULT_FILE_APPSETTING, string.Empty, true);

            string content = string.IsNullOrWhiteSpace(settings.Content) ? "{}" : settings.Content;
            JObject jsonSettings = JObject.Parse(content);
            DefaultInstance.AppSettings = jsonSettings;
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            Thread.Sleep(500);
            LoadAppSettings();
        }

        public static T GetConfig<T>(MixAppSettingsSection section, string name, T defaultValue = default)
        {
            var result = Instance.AppSettings[section.ToString()][name];
            if (result == null)
            {
                result = DefaultInstance.AppSettings[section.ToString()][name];
            }
            return result != null ? result.Value<T>() : defaultValue;
        }

        public static T GetEnumConfig<T>(MixAppSettingsSection section, string name)
        {
            Enum.TryParse(typeof(T), Instance.AppSettings[section.ToString()][name]?.Value<string>(), true, out object result);
            return result != null ? (T)result : default;
        }

        #region GlobalSettings

        public static T GetConfig<T>(string name)
        {
            return GetConfig<T>(MixAppSettingsSection.GlobalSettings, name);
        }
        
        public static T GetEnumConfig<T>(string name)
        {
            return GetEnumConfig<T>(MixAppSettingsSection.GlobalSettings, name);
        }

        #endregion

        public static void SetConfig<T>(MixAppSettingsSection section, string name, T value)
        {
            Instance.AppSettings[section.ToString()][name] = value != null ? JToken.FromObject(value) : null;
        }

        public static JObject GetGlobalSetting()
        {
            return JObject.FromObject(Instance.AppSettings[MixAppSettingsSection.GlobalSettings.ToString()]);
        }

        public static bool SaveSettings()
        {
            var settings = MixFileRepository.Instance.GetFile(MixConstants.CONST_FILE_APPSETTING, string.Empty, true, "{}");
            if (settings != null)
            {
                if (string.IsNullOrWhiteSpace(settings.Content))
                {
                    var defaultSettings = MixFileRepository.Instance.GetFile(MixConstants.CONST_DEFAULT_FILE_APPSETTING, string.Empty, true, "{}");
                    settings = new FileViewModel()
                    {
                        Filename = "appsettings",
                        Extension = MixFileExtensions.Json,
                        Content = defaultSettings.Content
                    };
                    return MixFileRepository.Instance.SaveFile(settings);
                }
                else
                {
                    JObject jsonSettings = JObject.Parse(settings.Content);

                    jsonSettings["ConnectionStrings"] = instance.AppSettings[MixAppSettingsSection.ConnectionStrings.ToString()];
                    jsonSettings["GlobalSettings"] = instance.AppSettings[MixAppSettingsSection.GlobalSettings.ToString()];
                    jsonSettings["GlobalSettings"]["LastUpdateConfiguration"] = DateTime.UtcNow;
                    jsonSettings["Authentication"] = instance.AppSettings[MixAppSettingsSection.Authentication.ToString()];
                    jsonSettings["IpSecuritySettings"] = instance.AppSettings[MixAppSettingsSection.IpSecuritySettings.ToString()];
                    jsonSettings["Smtp"] = instance.AppSettings[MixAppSettingsSection.Smtp.ToString()];
                    jsonSettings["MixConfigurations"] = instance.AppSettings[MixAppSettingsSection.MixConfigurations.ToString()];
                    settings.Content = jsonSettings.ToString();
                    return MixFileRepository.Instance.SaveFile(settings);
                }
            }
            else
            {
                return false;
            }
        }

        public static bool SaveSettings(string content)
        {
            var settings = MixFileRepository.Instance.GetFile(MixConstants.CONST_FILE_APPSETTING, string.Empty, true, "{}");

            settings.Content = content;
            return MixFileRepository.Instance.SaveFile(settings);
        }

        public static void Reload()
        {
            LoadAppSettings();
            MixCommonHelper.ReloadWebConfig();
        }
       
    }
}