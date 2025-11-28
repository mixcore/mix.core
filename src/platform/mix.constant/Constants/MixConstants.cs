namespace Mix.Constant.Constants
{
    public class MixConstants
    {
        public const string CONST_AUDIT_LOG_CONNECTION = "MixAuditLogConnection";
        public const string CONST_QUEUE_LOG_CONNECTION = "MixQueueLogConnection";
        public const string CONST_SETTINGS_CONNECTION = "SettingsConnection";
        public const string CONST_CMS_CONNECTION = "MixCmsConnection";
        public const string CONST_QUARTZ_CONNECTION = "MixQuartzConnection";
        public const string CONST_ACCOUNT_CONNECTION = "MixAccountConnection";
        public const string CONST_MIXDB_CONNECTION = "MixDbConnection";
        public const string CONST_MESSENGER_CONNECTION = "MixMessengerConnection";
        public const string CONST_SETTING_IS_MYSQL = "IsMysql";
        public const string CONST_SETTING_DATABASE_PROVIDER = "DatabaseProvider";
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
        public const string CONST_DEFAULT_AVATAR = "/mix-app/assets/img/user.png";
        public const string CONST_DEFAULT_MIX_CONTENT = "wwwroot/default-content.zip";
        public const string CONST_DEFAULT_EXTENSIONS_FILE_PATH = "\\Contents\\Extensions\\";
        public const string CONST_DEFAULT_EXTENSION_FILE_NAME = "extensions.json";
        public const string CONST_DEFAULT_STRING_ID = "default";
        public const int CONST_DEFAULT_PAGESIZE = 1000;
        public const string CONST_UPLOAD_FOLDER_DATE_FORMAT = "yyyy-MM";
        //public const string CONST_MIXDB_PREFIX = "mixdb_";
        public const string CONST_RSA_PUBLIC_KEY = "PublicKey";
        public const string CONST_RSA_PRIVATE_KEY = "PrivateKey";
        public const string CONST_PREFIX_ASSEMBLY = "mix";

        public static class EnvironmentKeys
        {
            public const string SERVICE_NAME = "SERVICE_NAME";
            public const string API_ENCRYPT_KEY = "API_ENCRYPT_KEY";
            public const string DEFAULT_CULTURE = "DEFAULT_CULTURE";
            public const string ASPNETCORE_ENVIRONMENT = "ASPNETCORE_ENVIRONMENT";
            public const string SETTINGS_CONNECTION_STRING = "SETTINGS_CONNECTION_STRING";
            public const string DATABASE_PROVIDER = "DATABASE_PROVIDER";
            public const string IS_HTTPS = "IS_HTTPS";
            public const string IS_INIT = "IS_INIT";
            public const string INIT_STATUS = "INIT_STATUS";
            public const string ALLOW_ANY_ORGIN = "ALLOW_ANY_ORGIN";
            public const string REPONSE_CACHE_IN_SECOND = "REPONSE_CACHE_IN_SECOND";
        }

        public static class GlobalSettingNames
        {
            public const string Endpoint = "endpoint";
            public const string Portal = "portal";
            public const string Authentication = "authentication";
            public const string Smtp = "smtp";
            public const string Database = "database";
            public const string Heart = "mix_heart";
            public const string Ip = "ip";
            public const string Translator = "translator";
            public const string Google = "goole";
            public const string FirebaseCredential = "firebase_credential";
            public const string GcsCredential = "gcs_credential";
        }

        public static class TemplateFolder
        {
            public const string Masters = "Masters";
            public const string Layouts = "Layouts";
            public const string Pages = "Pages";
            public const string Posts = "Posts";
            public const string Modules = "Modules";
            public const string Forms = "Forms";
            public const string Edms = "Edms";
        }

        public static class DefaultTemplate
        {
            public const string Master = "_Layout.cshtml";
            public const string Page = "_Blank.cshtml";
            public const string Post = "Default.cshtml";
            public const string Module = "_Blank.cshtml";
        }
    }
}