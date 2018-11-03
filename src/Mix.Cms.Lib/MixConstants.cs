using System;
using System.Collections.Generic;
using System.Text;

namespace Mix.Cms.Lib
{
    public class MixConstants
    {
        public const string CONST_CMS_CONNECTION = "MixCmsConnection";
        public const string CONST_ACCOUNT_CONNECTION = "MixAccountConnection";
        public const string CONST_SETTING_IS_SQLITE = "IsSqlite";
        public const string CONST_SETTING_LANGUAGE = "Language";
        public const string CONST_CHAT_CONNECTION = "ChatConnection";
        public const string CONST_FILE_APPSETTING = "mixCmsSettings.json";
        public const string CONST_FILE_CONFIGURATIONS = "configurations.json";
        public const string CONST_FILE_LANGUAGES = "languages.json";
        public const string CONST_PATH_HOME_ACCESS_DENIED = "/home/access-denied";
        public const string CONST_PATH_HOME_ERROR = "/Home/Error";
        public const string CONST_SECTION_LOGGING = "Logging";
        public const string CONST_ROUTE_DEFAULT = "default";
        public const string CONST_APPID = "SetYourDataHere";
        public const string CONST_APPSECRET = "SetYourDataHere";
        public const string CONST_DOMAIN_NOTIFICATION_KEY_COMMIT = "Commit";
        public const string CONST_DOMAIN_NOTIFICATION_KEY_COMMIT_VALUE = "We had a problem during saving your data.";
        public const string CONST_DOMAIN_NOTIFICATION = "DomainNotification";
        public const string CONST_DEFAULT_EXTENSIONS_FILE_PATH = "\\Contents\\Extensions\\";
        public const string CONST_DEFAULT_EXTENSION_FILE_NAME = "extensions.json";

        public static List<string> cachedKeys = new List<string>();

        public static class ConfigurationKeyword
        {
            public const string ConnectionString = "ConnectionString";
            public const string OrderBy = "OrderBy";
            public const string DefaultTemplateFolder = "DefaultTemplateFolder";
            public const string Language = "Language";
            public const string DefaultStatus = "DefaultStatus";
            public const string DefaultCulture = "DefaultCulture";
            public const string IsSqlite = "IsSqlite";
            public const string ThemeId = "ThemeId";
            public const string ThemeName= "ThemeName";
            public const string ApiEncryptKey = "ApiEncryptKey";
            public const string ApiEncryptIV = "ApiEncryptIV";
            public const string IsEncryptApi = "IsEncryptApi";
            public const string TemplateExtension = "TemplateExtension";
            public const string DefaultTheme= "DefaultTheme";
            public const string DefaultTemplate = "DefaultTemplate";
            public const string DefaultTemplateContent = "DefaultTemplateContent";
            public const string DefaultContentStatus = "DefaultContentStatus";
        }

        public static class Folder
        {
            public const string FileFolder = @"Content";
            public const string TemplateExtension = @".cshtml";
            public const string TemplatesAssetFolder = @"Templates";
            public const string TemplatesFolder = @"Views/Shared/Templates";
            public const string UploadFolder = @"Content/Uploads";
            public const string WebRootPath = @"wwwroot";
        }

    }
}
