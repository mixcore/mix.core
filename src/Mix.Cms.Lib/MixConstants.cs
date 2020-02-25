using System.Collections.Generic;

namespace Mix.Cms.Lib
{
    public class MixConstants
    {
        public const string CONST_CMS_CONNECTION = "MixCmsConnection";
        public const string CONST_MESSENGER_CONNECTION = "MixMessengerConnection";
        public const string CONST_ACCOUNT_CONNECTION = "MixAccountConnection";
        public const string CONST_SETTING_IS_MYSQL = "IsMysql";
        public const string CONST_SETTING_DATABASE_PROVIDER = "DatabaseProvider";
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

        public static List<string> cachedKeys = new List<string>();

        public static class AttributeSetName
        {
            public const string ADDITIONAL_FIELD_PAGE = "sys_additional_field_page";
            public const string ADDITIONAL_FIELD_POST = "sys_additional_field_post";
            public const string ADDITIONAL_FIELD_MODULE = "sys_additional_field_module";
            public const string NAVIGATION = "sys_navigation";
            public const string MENU_ITEM = "sys_menu_item";
            public const string SYSTEM_CATEGORY = "sys_category";
            public const string SYSTEM_TAG = "sys_tag";
        }

        public static class ConfigurationKeyword
        {
            public const string ConnectionString = "ConnectionString";
            public const string OrderBy = "OrderBy";
            public const string DefaultBlankTemplateFolder = "DefaultTemplateFolder";
            public const string DefaultTemplateFolder = "DefaultTemplateFolder";
            public const string Language = "Language";
            public const string DefaultStatus = "DefaultStatus";
            public const string DefaultCulture = "DefaultCulture";
            public const string IsMysql = "IsMysql";
            public const string Domain = "Domain";
            public const string PortalThemeSettings = "PortalThemeSettings";
            public const string ThemeId = "ThemeId";
            public const string ThemeName = "ThemeName";
            public const string ThemeFolder = "ThemeFolder";
            public const string ApiEncryptKey = "ApiEncryptKey";
            public const string ApiEncryptIV = "ApiEncryptIV";
            public const string IsEncryptApi = "IsEncryptApi";
            public const string TemplateExtension = "TemplateExtension";
            public const string DefaultTheme = "DefaultTheme";
            public const string DefaultTemplate = "DefaultTemplate";
            public const string DefaultTemplateContent = "DefaultTemplateContent";
            public const string DefaultContentStatus = "DefaultContentStatus";
            public const string NextSyncContent = "NextSyncContent";
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
            public const string Master = "_Layout";
            public const string Page = "_Blank.cshtml";
            public const string Post = "_Blank.cshtml";
            public const string Module = "_Blank.cshtml";
        }

        public static class Folder
        {
            public const string FileFolder = @"content";
            public const string CacheFolder = @"cache";
            public const string TemplateExtension = @".cshtml";
            public const string TemplatesAssetFolder = @"templates";
            public const string TemplatesFolder = @"Views/Shared/Templates";
            public const string UploadFolder = @"Content/Uploads";
            public const string WebRootPath = @"wwwroot";
        }

        public class ServiceHub
        {
            public const string ReceiveMethod = "receive_message";
            public const string HubMemberName = "hub_member";
            public const string HubMemberFieldName = "hub_name";
            public const string DefaultDevice = "website";
            public const string UnknowErrorMsg = "Unknow";

            public const string SaveData = "save_data";
            public const string JoinGroup = "join_group";
            public const string NewMessage = "new_message";
            public const string NewNotification = "new_notification";
            public const string NewMember = "new_member";
        }
    }
}