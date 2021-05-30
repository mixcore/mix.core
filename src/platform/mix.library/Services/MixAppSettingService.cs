using Microsoft.EntityFrameworkCore.Storage;
using Mix.Lib.Constants;
using Mix.Lib.Entities.Cms;
using Mix.Common.Helper;
using Mix.Heart.Enums;
using Mix.Heart.Models;
using Mix.Infrastructure.Repositories;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using Mix.Identity.Models;
using Mix.Lib.Entities.Account;
using Mix.Lib.Enums;

namespace Mix.Lib.Services
{
    public class MixAppSettingService
    {
        /// <summary>
        /// The synchronize root
        /// </summary>
        private static readonly object syncRoot = new Object();

        /// <summary>
        /// The instance
        /// </summary>
        private static volatile MixAppSettingService instance;

        private static volatile MixAppSettingService defaultInstance;

        private JObject AppSettings { get; set; }
        private readonly FileSystemWatcher watcher = new FileSystemWatcher();
        public MixAuthenticationConfigurations MixAuthentications { get => instance.AppSettings[MixAppSettingsSection.Authentication.ToString()].ToObject<MixAuthenticationConfigurations>(); }

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
                            instance.LoadAppSettings();
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
                            defaultInstance.LoadDefaultAppSettings();
                        }
                    }
                }

                return defaultInstance;
            }
        }

        private void LoadAppSettings()
        {
            // Load configurations from appSettings.json
            JObject jsonSettings = new JObject();
            var settings = MixFileRepository.Instance.GetFile(MixConstants.CONST_FILE_APPSETTING, string.Empty, true);

            if (string.IsNullOrEmpty(settings.Content))
            {
                settings = MixFileRepository.Instance.GetFile(MixConstants.CONST_DEFAULT_FILE_APPSETTING, string.Empty, true, "{}");
            }

            string content = string.IsNullOrWhiteSpace(settings.Content) ? "{}" : settings.Content;
            jsonSettings = JObject.Parse(content);

            instance.AppSettings = jsonSettings;
            MixCommonHelper.WebConfigInstance = jsonSettings;
        }

        private void LoadDefaultAppSettings()
        {
            // Load configurations from appSettings.json
            JObject jsonSettings = new JObject();
            var settings = MixFileRepository.Instance.GetFile(MixConstants.CONST_DEFAULT_FILE_APPSETTING, string.Empty, true);

            string content = string.IsNullOrWhiteSpace(settings.Content) ? "{}" : settings.Content;
            jsonSettings = JObject.Parse(content);
            DefaultInstance.AppSettings = jsonSettings;
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            Thread.Sleep(500);
            Instance.LoadAppSettings();
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

        public static T GetConfig<T>(MixAppSettingsSection section, string culture, string name)
        {
            JToken result = null;
            if (!string.IsNullOrEmpty(culture) && Instance.AppSettings[section.ToString()][culture] != null)
            {
                result = Instance.AppSettings[section.ToString()][culture][name];
            }
            return result != null ? result.Value<T>() : default;
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
        
        public static T GetConfig<T>(string culture, string name)
        {
            return GetConfig<T>(MixAppSettingsSection.GlobalSettings, culture, name);
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


        public static void SetConfig<T>(MixAppSettingsSection section, string culture, string name, T value)
        {
            Instance.AppSettings[section.ToString()][culture][name] = value.ToString();
        }

       
        public static JObject GetTranslator(string culture)
        {
            return JObject.FromObject(Instance.AppSettings[MixAppSettingsSection.Translator.ToString()][culture] ?? new JObject());
        }

        public static JObject GetLocalizeSettings(string culture)
        {
            return JObject.FromObject(Instance.AppSettings[MixAppSettingsSection.LocalSettings.ToString()][culture] ?? new JObject());
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
                    jsonSettings["Translator"] = instance.AppSettings[MixAppSettingsSection.Translator.ToString()];
                    jsonSettings["LocalSettings"] = instance.AppSettings[MixAppSettingsSection.LocalSettings.ToString()];
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
            Instance.LoadAppSettings();
            MixCommonHelper.ReloadWebConfig();
        }

        public static void LoadFromDatabase(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction
                , out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                var translator = new JObject();
                var ListLanguage = context.MixLanguage.ToList();
                var cultures = context.MixCulture.ToList();
                foreach (var culture in cultures)
                {
                    JObject arr = new JObject();
                    foreach (var lang in ListLanguage.Where(l => l.Specificulture == culture.Specificulture).ToList())
                    {
                        JProperty l = new JProperty(lang.Keyword, lang.Value ?? lang.DefaultValue);
                        arr.Add(l);
                    }
                    translator.Add(new JProperty(culture.Specificulture, arr));
                }
                Instance.AppSettings[MixAppSettingsSection.Translator] = translator;

                var localConfigurations = new JObject();
                var listLocalSettings = context.MixConfiguration.ToList();
                foreach (var culture in cultures)
                {
                    JObject arr = new JObject();
                    foreach (var cnf in listLocalSettings.Where(l => l.Specificulture == culture.Specificulture).ToList())
                    {
                        JProperty l = new JProperty(cnf.Keyword, cnf.Value);
                        arr.Add(l);
                    }
                    localConfigurations.Add(new JProperty(culture.Specificulture, arr));
                }
                Instance.AppSettings[MixAppSettingsSection.LocalSettings] = localConfigurations;
                UnitOfWorkHelper<MixCmsContext>.HandleTransaction(true, isRoot, transaction);
            }
            catch (Exception ex) // TODO: Add more specific exeption types instead of Exception only
            {
                var error = UnitOfWorkHelper<MixCmsContext>.HandleException<MixLanguage>(ex, isRoot, transaction);
            }
            finally
            {
                //if current Context is Root
                if (isRoot)
                {
                    UnitOfWorkHelper<MixCmsContext>.CloseDbContext(ref context, ref transaction);
                }
            }
        }





        public static string GetTemplateFolder(string culture)
        {
            return $"{MixFolders.SiteContentAssetsFolder}/{Instance.AppSettings[MixAppSettingsSection.LocalSettings][culture][MixAppSettingKeywords.ThemeFolder]}";
        }

        public static string GetTemplateUploadFolder(string culture)
        {
            return $"{MixFolders.SiteContentAssetsFolder}/" +
                $"{Instance.AppSettings[MixAppSettingsSection.LocalSettings][culture][MixAppSettingKeywords.ThemeFolder]}/" +
                $"uploads/" +
                $"{DateTime.UtcNow.ToString(MixConstants.CONST_UPLOAD_FOLDER_DATE_FORMAT)}";
        }

        public static MixCmsContext GetDbContext()
        {
            var provider = MixAppSettingService.GetEnumConfig<MixDatabaseProvider>(MixAppSettingsSection.GlobalSettings, MixConstants.CONST_SETTING_DATABASE_PROVIDER);
            switch (provider)
            {
                case MixDatabaseProvider.MSSQL:
                    return new MsSqlMixCmsContext();

                case MixDatabaseProvider.MySQL:
                    return new MySqlMixCmsContext();

                case MixDatabaseProvider.SQLITE:
                    return new SqliteMixCmsContext();

                case MixDatabaseProvider.PostgreSQL:
                    return new PostgresqlMixCmsContext();

                default:
                    return null;
            }
        }
        public static MixCmsAccountContext GetAccountDbContext()
        {
            var provider = GetEnumConfig<MixDatabaseProvider>(MixAppSettingsSection.GlobalSettings, MixConstants.CONST_SETTING_DATABASE_PROVIDER);
            switch (provider)
            {
                case MixDatabaseProvider.MSSQL:
                case MixDatabaseProvider.MySQL:
                case MixDatabaseProvider.SQLITE:
                    return new SQLAccountContext();

                case MixDatabaseProvider.PostgreSQL:
                    return new PostgresSQLAccountContext();

                default:
                    return null;
            }
        }
    }
}