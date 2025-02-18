using Microsoft.Extensions.Configuration;
using Mix.Database.Entities.Settings;
using Mix.Heart.Extensions;
using Mix.Heart.Helpers;
using Mix.Shared.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using Mix.Lib.Extensions;

namespace Mix.Database.Services.MixGlobalSettings
{
    public abstract class GlobalSettingServiceBase<TAppSetting>
        where TAppSetting : class
    {
        public TAppSetting AppSettings { get; set; }
        public JObject RawSettings;
        protected AppSettingsService AppSettingsService;
        protected string _sectionName;
        protected bool _isEncrypt;
        protected MixGlobalSetting _settings;

        protected string _aesKey;
        protected readonly IConfiguration _configuration;

        protected GlobalSettingServiceBase(IConfiguration configuration, MixGlobalSetting settings)
        {
            _configuration = configuration;
            _aesKey = _configuration.AesKey();
            _sectionName = settings.SectionName;
            AppSettingsService = new AppSettingsService(configuration);
            _settings = settings;
            LoadAppSettings();
        }
        public virtual T GetConfig<T>(string name, T defaultValue = default)
        {
            var result = RawSettings[name];
            return result != null ? result.Value<T>() : defaultValue;
        }
        public virtual void SetConfig<TValue>(string name, TValue value, bool isSave = false)
        {
            if (string.IsNullOrEmpty(_sectionName))
            {
                RawSettings[name] = value != null ? JToken.FromObject(value) : null;
                _configuration.GetSection(_sectionName)[name] = value.ToString();
            }
            else
            {
                RawSettings[_sectionName][name] = value != null ? JToken.FromObject(value) : null;
            }
            if (isSave)
            {
                SaveSettings();
            }
        }

        public virtual void SaveSettings()
        {
            using var dbContext = GetSettingDbContext();
            {
                if (dbContext.Database.GetPendingMigrations().Any())
                {
                    dbContext.Database.Migrate();
                }
                var settings = dbContext.MixGlobalSetting.FirstOrDefault(m => m.SystemName == _settings.SystemName);
                settings.Settings = RawSettings.ToString(Formatting.None);
                dbContext.MixGlobalSetting.Update(settings);
                dbContext.SaveChanges();
            }
        }

        protected virtual void LoadAppSettings()
        {
            AppSettings = string.IsNullOrEmpty(_sectionName) ? _configuration.Get<TAppSetting>()
                : _configuration.GetSection(_sectionName).Get<TAppSetting>();
            var content = _settings.IsEncrypt ? AesEncryptionHelper.DecryptString(_settings.Settings, _aesKey)
            : _settings.Settings;
            RawSettings = JObject.Parse(content);
        }

        protected GlobalSettingContext GetSettingDbContext()
        {
            return _configuration.DatabaseProvider() switch
            {
                MixDatabaseProvider.SQLSERVER => new SqlServerGlobalSettingContext(_configuration),
                MixDatabaseProvider.MySQL => new MySqlGlobalSettingContext(_configuration),
                MixDatabaseProvider.SQLITE => new SqliteGlobalSettingContext(_configuration),
                MixDatabaseProvider.PostgreSQL => new PostgresGlobalSettingContext(_configuration),
                _ => throw new NotImplementedException()
            };
        }
    }
}
