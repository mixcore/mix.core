using System.Collections.Generic;

namespace Mix.Cms.Lib.Constants
{
    public static class MixConfigurations
    {
        public const string CONST_CMS_CONNECTION = "MixCmsConnection";
        public const string CONST_MESSENGER_CONNECTION = "MixMessengerConnection";
        public const string CONST_ACCOUNT_CONNECTION = "MixAccountConnection";
        public const string CONST_SETTING_IS_MYSQL = "IsMysql";
        public const string CONST_SETTING_DATABASE_PROVIDER = "DatabaseProvider";
        public const string CONST_MIXCORE_VERSION = "MixcoreVersion";
        public const string CONST_SETTING_LANGUAGE = "Language";
        public const string CONST_FILE_APPSETTING = "appsettings.json";
        public const string CONST_DEFAULT_FILE_APPSETTING = "default.appsettings.json";
        public const string CONST_FILE_CONFIGURATIONS = "configurations.json";
        public const string CONST_FILE_ATTRIBUTE_SETS = "attribute_sets.json";
        public const string CONST_FILE_POSITIONS = "menu-positions.json";
        public const string CONST_FILE_LANGUAGES = "languages.json";
        public const string CONST_FILE_CULTURES = "cultures.json";
        public const string CONST_FILE_PAGES = "pages.json";
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
        public const string CONST_DEFAULT_STRING_ID = "default";
        public const string CONST_UPLOAD_FOLDER_FORMAT = "MMM-yyyy";
        public static List<string> cachedKeys = new List<string>();
    }
}