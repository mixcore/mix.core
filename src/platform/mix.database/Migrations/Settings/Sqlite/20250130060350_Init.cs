using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mix.Database.Migrations.Settings.Sqlite
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "mix_global_setting",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    last_modified = table.Column<DateTime>(type: "datetime", nullable: true),
                    service_name = table.Column<string>(type: "varchar(250)", nullable: false, collation: "NOCASE"),
                    section_name = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    display_name = table.Column<string>(type: "varchar(250)", nullable: false, collation: "NOCASE"),
                    description = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "NOCASE"),
                    tenant_id = table.Column<int>(type: "INTEGER", nullable: false),
                    system_name = table.Column<string>(type: "varchar(250)", nullable: true),
                    settings = table.Column<string>(type: "text", nullable: true),
                    is_encrypt = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MixGlobalSetting", x => x.id);
                });

            migrationBuilder.InsertData("mix_global_setting",
                columns: new[] { "id", "last_modified", "service_name", "section_name", "display_name", "description", "tenant_id", "system_name", "settings", "is_encrypt" },
                values: new object[] { 1, null, "portal", null, "Rate Limit", "Rate limit", 1, "rate_limit", "{\"MixRateLimit\":{\"PermitLimit\":30,\"Window\":10,\"ReplenishmentPeriod\":2,\"QueueLimit\":1000,\"SegmentsPerWindow\":8,\"TokenLimit\":10,\"TokenLimit2\":20,\"TokensPerPeriod\":4,\"AutoReplenishment\":false}}", false });
            migrationBuilder.InsertData("mix_global_setting",
                columns: new[] { "id", "last_modified", "service_name", "section_name", "display_name", "description", "tenant_id", "system_name", "settings", "is_encrypt" },
                values: new object[] { 2, null, "portal", null, "Portal", "Portal", 1, "portal", "{\"primaryColorHue\":230,\"primaryColorSaturation\":35,\"primaryColorBrightness\":50,\"bgColor\":\"#fbfbfb\",\"textColor\":\"#404040\",\"primaryColor\":\"#47c5d1\",\"bgColorHover\":\"#d1e0de\",\"borderColor\":\"#d1e0de\",\"borderColorHover\":\"#e0e4e7\",\"linkColor\":\"#47c5d1\",\"linkColorHover\":\"#404040\",\"linkColorActive\":\"#404040\",\"textColorHover\":\"#008b8b\",\"fontFamily\":\"\",\"fontSizeH1\":\"16\",\"fontSize\":\"12\"}", false });
            migrationBuilder.InsertData("mix_global_setting",
                columns: new[] { "id", "last_modified", "service_name", "section_name", "display_name", "description", "tenant_id", "system_name", "settings", "is_encrypt" },
                values: new object[] { 3, null, "portal", null, "Ip Config", "Ip", 1, "ip", "{\"IsRetrictIp\":true,\"AllowedPortalIps\":[],\"AllowedIps\":[],\"ExceptIps\":[]}", false });
            migrationBuilder.InsertData("mix_global_setting",
                columns: new[] { "id", "last_modified", "service_name", "section_name", "display_name", "description", "tenant_id", "system_name", "settings", "is_encrypt" },
                values: new object[] { 4, null, "portal", null, "Translator", "translator", 1, "translator", "{}", false });
            migrationBuilder.InsertData("mix_global_setting",
                columns: new[] { "id", "last_modified", "service_name", "section_name", "display_name", "description", "tenant_id", "system_name", "settings", "is_encrypt" },
                values: new object[] { 5, null, "portal", null, "Storage", "Storage", 1, "storage", "{\"StorageSetting\":{\"Provider\":\"MIX\",\"CloudFlareSetting\":{\"EndpointTemplate\":\"https://api.cloudflare.com/client/v4/accounts/{0}/images/v1\",\"ApiToken\":\"\",\"ApiKey\":{\"GlobalKey\":\"\",\"OriginKey\":\"\"},\"ZoneId\":\"\",\"AccountId\":\"\"},\"AzureStorageSetting\":{\"CdnUrl\":\"\",\"StorageAccountName\":\"\",\"AzureWebJobStorage\":\"\",\"ContainerName\":\"\"},\"AwsSetting\":{\"BucketName\":\"\",\"CloudFrontUrl\":\"\",\"BucketUrl\":\"\",\"AccessKeyId\":\"\",\"SecretAccessKey\":\"\",\"Region\":\"\"},\"MixSetting\":{},\"IsAutoScaleImage\":true,\"ImageSizes\":[{\"name\":\"XXS\",\"width\":30},{\"name\":\"XS\",\"width\":100},{\"name\":\"S\",\"width\":300},{\"name\":\"M\",\"width\":600},{\"name\":\"L\",\"width\":1000},{\"name\":\"XL\",\"width\":2000},{\"name\":\"XXL\",\"width\":4000}]}}", false });
            migrationBuilder.InsertData("mix_global_setting",
                columns: new[] { "id", "last_modified", "service_name", "section_name", "display_name", "description", "tenant_id", "system_name", "settings", "is_encrypt" },
                values: new object[] { 6, null, "portal", null, "Global", "Global Settings", 1, "global", "{\"GlobalSettings\":{\"IsInit\":true,\"IsUpdateSystemDatabases\":false,\"EnableAuditLog\":true,\"IsLogStream\":false,\"AllowAnyOrigin\":false,\"IsHttps\":false,\"ResponseCache\":5,\"AesKey\":null,\"DefaultDomain\":\"\",\"EnableOcelot\":false,\"InitStatus\":\"Blank\"}}", false });
            migrationBuilder.InsertData("mix_global_setting",
                columns: new[] { "id", "last_modified", "service_name", "section_name", "display_name", "description", "tenant_id", "system_name", "settings", "is_encrypt" },
                values: new object[] { 7, null, "portal", null, "Log", "Log", 1, "log", "{\"Log\":{\"IsLogStream\":true,\"EnableAuditLog\":true,\"EnableAuditLogResponse\":true}}", false });
            migrationBuilder.InsertData("mix_global_setting",
                columns: new[] { "id", "last_modified", "service_name", "section_name", "display_name", "description", "tenant_id", "system_name", "settings", "is_encrypt" },
                values: new object[] { 8, null, "portal", null, "Payment", "Payment", 1, "payment", "{\"Payments\":{\"Payoo\":null}}", false });
            migrationBuilder.InsertData("mix_global_setting",
                columns: new[] { "id", "last_modified", "service_name", "section_name", "display_name", "description", "tenant_id", "system_name", "settings", "is_encrypt" },
                values: new object[] { 9, null, "portal", null, "Authentication", "Authentication", 1, "authentication", "{\"Authentication\":{\"AccessTokenExpiration\":20,\"RefreshTokenExpiration\":120,\"ValidateIssuer\":false,\"ValidateAudience\":false,\"ValidateLifetime\":true,\"ValidateIssuerSigningKey\":true,\"TokenType\":\"Bearer\",\"Audience\":\"mix-core\",\"ClientId\":\"00000000-0000-0000-0000-000000000000\",\"SecretKey\":\"61cf4651c35a4dfab0bb2cea9181db53\",\"Issuer\":\"mix-core\",\"Issuers\":\"mix-core\",\"Audiences\":\"mix-core\",\"RequireUniqueEmail\":false,\"RequireConfirmedEmail\":false,\"ConfirmedEmailUrl\":null,\"ConfirmedEmailUrlSuccess\":null,\"ConfirmedEmailUrlFail\":null,\"TokenLifespan\":120,\"Facebook\":{\"AppId\":\"\",\"AppSecret\":\"\"},\"Google\":{\"AppId\":null,\"AppSecret\":null},\"Microsoft\":{\"AppId\":null,\"AppSecret\":null},\"Twitter\":{\"AppId\":null,\"AppSecret\":null},\"AzureAd\":{\"Instance\":\"https://login.microsoftonline.com/\",\"ClientId\":null,\"TenantId\":null,\"Scopes\":null}},\"OAuth\":{\"Endpoint\":\"/api/v2/rest/auth/connect/token\",\"ClientId\":null,\"ClientSecret\":null,\"GrantType\":\"client_credentials\",\"Scope\":[]}}", false });
            migrationBuilder.InsertData("mix_global_setting",
                columns: new[] { "id", "last_modified", "service_name", "section_name", "display_name", "description", "tenant_id", "system_name", "settings", "is_encrypt" },
                values: new object[] { 10, null, "portal", null, "SMTP", "SMTP", 1, "smtp", "{\"Smtp\":{\"Server\":null,\"Port\":587,\"SSL\":true,\"User\":null,\"Password\":null,\"From\":null,\"FromName\":null}}", false });
            migrationBuilder.InsertData("mix_global_setting",
                columns: new[] { "id", "last_modified", "service_name", "section_name", "display_name", "description", "tenant_id", "system_name", "settings", "is_encrypt" },
                values: new object[] { 11, null, "portal", "Google", "Google", "Google", 1, "google", "{\"Google\":{\"ProjectId\":null,\"Firebase\":{\"WebApiKey\":\"\",\"AuthDomain\":\"\",\"StorageBucket\":\"\",\"MessagingSenderId\":\"\",\"AppId\":\"\",\"Credential\":null},\"Storage\":{\"BucketName\":null,\"Credential\":null}}}", false });
            migrationBuilder.InsertData("mix_global_setting",
                columns: new[] { "id", "last_modified", "service_name", "section_name", "display_name", "description", "tenant_id", "system_name", "settings", "is_encrypt" },
                values: new object[] { 12, null, "portal", "Azure", "Azure", "Azure", 1, "azure", "{\"Azure\":{\"SignalRConnectionString\":null}}", false });
            migrationBuilder.InsertData("mix_global_setting",
                columns: new[] { "id", "last_modified", "service_name", "section_name", "display_name", "description", "tenant_id", "system_name", "settings", "is_encrypt" },
                values: new object[] { 13, null, "portal", null, "Redis", "Redis", 1, "redis", "{\"Redis\":{\"ConnectionStrings\":null,\"SlidingExpirationInMinute\":10,\"AbsoluteExpirationInMinute\":null,\"AbsoluteExpirationRelativeToNowInMinute\":null}}", false });
            migrationBuilder.InsertData("mix_global_setting",
                columns: new[] { "id", "last_modified", "service_name", "section_name", "display_name", "description", "tenant_id", "system_name", "settings", "is_encrypt" },
                values: new object[] { 14, null, "portal", null, "Mix Heart", "Mix Heart", 1, "mix_heart", "{\"CacheConnection\":null,\"IsCache\":true,\"CacheMode\":\"HYBRID\",\"CacheFolder\":\"wwwroot/mixcontent/cache\"}", false });
            migrationBuilder.InsertData("mix_global_setting",
                columns: new[] { "id", "last_modified", "service_name", "section_name", "display_name", "description", "tenant_id", "system_name", "settings", "is_encrypt" },
                values: new object[] { 15, null, "portal", null, "Ocelot", "Ocelot", 1, "ocelot", "{\"Routes\":[{\"Priority\":0,\"DownstreamPathTemplate\":\"/{everything}\",\"DownstreamScheme\":\"https\",\"DownstreamHostAndPorts\":[{\"Host\":\"localhost\",\"Port\":5010}],\"UpstreamPathTemplate\":\"/{everything}\",\"UpstreamHttpMethod\":[\"Get\",\"Post\",\"Put\",\"Patch\",\"Delete\"],\"RateLimitOptions\":{\"ClientWhitelist\":[],\"EnableRateLimiting\":true,\"Period\":\"1s\",\"PeriodTimespan\":1,\"Limit\":1000},\"FileCacheOptions\":{\"TtlSeconds\":0,\"Region\":\"Portal_VN\"},\"HttpHandlerOptions\":{\"AllowAutoRedirect\":true,\"UseCookieContainer\":true,\"UseTracing\":true,\"MaxConnectionsPerServer\":1000}}],\"GlobalConfiguration\":{\"BaseUrl\":\"\"}}", false });
            migrationBuilder.InsertData("mix_global_setting",
                columns: new[] { "id", "last_modified", "service_name", "section_name", "display_name", "description", "tenant_id", "system_name", "settings", "is_encrypt" },
                values: new object[] { 16, null, "portal", null, "Endpoints", "Endpoints", 1, "endpoint", "{\"Account\":\"https://localhost:5010\",\"Common\":null,\"Portal\":null,\"Theme\":null,\"Mixcore\":\"https://localhost:5010\",\"Messenger\":\"https://localhost:5010\",\"Scheduler\":null,\"Grpc\":null,\"MixMq\":\"https://localhost:5010\",\"LocalGateway\":\"https://localhost:7142\",\"Storage\":\"\",\"UniversalPortal\":\"https://universal.p4ps.net\",\"UniversalPortalDev\":\"https://mix-portal-angular.vercel.app\",\"LocalPortalDev\":\"http://localhost:4200\",\"DataVerse\":\"https://mix-portal-angular-phongcaos-projects.vercel.app\"}", false });
            migrationBuilder.InsertData("mix_global_setting",
                columns: new[] { "id", "last_modified", "service_name", "section_name", "display_name", "description", "tenant_id", "system_name", "settings", "is_encrypt" },
                values: new object[] { 17, null, "portal", null, "Database", "Database", 1, "database", "{\"ConnectionStrings\":{\"SettingsConnection\":\"Data Source=wwwroot/mixcontent/settings.sqlite\",\"MixAccountConnection\":null,\"MixAuditLogConnection\":null,\"MixQueueLogConnection\":null,\"MixCmsConnection\":null,\"MixQuartzConnection\":null,\"MixDbConnection\":null},\"ClearDbPool\":false,\"DatabaseProvider\":\"SQLITE\"}", false });
            migrationBuilder.InsertData("mix_global_setting",
                columns: new[] { "id", "last_modified", "service_name", "section_name", "display_name", "description", "tenant_id", "system_name", "settings", "is_encrypt" },
                values: new object[] { 18, null, "portal", null, "Queue", "Queue", 1, "queue", "{\"MessageQueueSetting\":{\"Provider\":\"MIX\",\"GoogleQueueSetting\":{\"CredentialFile\":\"\",\"ProjectId\":\"\"},\"AzureServiceBus\":{\"ConnectionStrings\":\"\",\"QueueName\":\"\"},\"Mix\":{\"ProjectId\":\"\"},\"RabitMqQueueSetting\":{\"HostName\":\"localhost\",\"UserName\":null,\"Password\":null,\"VHost\":null,\"Port\":5672}}}", false });
            migrationBuilder.InsertData("mix_global_setting",
                columns: new[] { "id", "last_modified", "service_name", "section_name", "display_name", "description", "tenant_id", "system_name", "settings", "is_encrypt" },
                values: new object[] { 19, null, "gateway", null, "Ocelot", "Ocelot", 1, "ocelot", "{\"Routes\":[{\"Priority\":0,\"DownstreamPathTemplate\":\"/{everything}\",\"DownstreamScheme\":\"https\",\"DownstreamHostAndPorts\":[{\"Host\":\"localhost\",\"Port\":5010}],\"UpstreamPathTemplate\":\"/{everything}\",\"UpstreamHttpMethod\":[\"Get\",\"Post\",\"Put\",\"Patch\",\"Delete\"],\"RateLimitOptions\":{\"ClientWhitelist\":[],\"EnableRateLimiting\":true,\"Period\":\"1s\",\"PeriodTimespan\":1,\"Limit\":1000},\"FileCacheOptions\":{\"TtlSeconds\":0,\"Region\":\"Portal_VN\"},\"HttpHandlerOptions\":{\"AllowAutoRedirect\":true,\"UseCookieContainer\":true,\"UseTracing\":true,\"MaxConnectionsPerServer\":1000}},{\"Priority\":2,\"DownstreamPathTemplate\":\"/pos/{everything}\",\"DownstreamScheme\":\"https\",\"DownstreamHostAndPorts\":[{\"Host\":\"localhost\",\"Port\":7119}],\"UpstreamPathTemplate\":\"/pos/{everything}\",\"UpstreamHttpMethod\":[\"Get\",\"Post\",\"Put\",\"Patch\",\"Delete\"],\"RateLimitOptions\":{\"ClientWhitelist\":[],\"EnableRateLimiting\":true,\"Period\":\"1s\",\"PeriodTimespan\":1,\"Limit\":10},\"FileCacheOptions\":{\"TtlSeconds\":0,\"Region\":\"Pos_VN\"},\"HttpHandlerOptions\":{\"AllowAutoRedirect\":true,\"UseCookieContainer\":true,\"UseTracing\":true,\"MaxConnectionsPerServer\":20}},{\"Priority\":1,\"DownstreamPathTemplate\":\"/api/v2/rest/auth/{everything}\",\"DownstreamScheme\":\"https\",\"DownstreamHostAndPorts\":[{\"Host\":\"localhost\",\"Port\":7016}],\"UpstreamPathTemplate\":\"/api/v2/rest/auth/{everything}\",\"UpstreamHttpMethod\":[\"Get\",\"Post\",\"Put\",\"Patch\",\"Delete\"],\"RateLimitOptions\":{\"ClientWhitelist\":[],\"EnableRateLimiting\":true,\"Period\":\"1s\",\"PeriodTimespan\":1,\"Limit\":10},\"FileCacheOptions\":{\"TtlSeconds\":0,\"Region\":\"Auth_VN\"},\"HttpHandlerOptions\":{\"AllowAutoRedirect\":true,\"UseCookieContainer\":true,\"UseTracing\":true,\"MaxConnectionsPerServer\":20}},{\"Priority\":5,\"DownstreamPathTemplate\":\"/api/p4ps/rms/{everything}\",\"DownstreamScheme\":\"https\",\"DownstreamHostAndPorts\":[{\"Host\":\"localhost\",\"Port\":7080}],\"UpstreamPathTemplate\":\"/api/p4ps/rms/{everything}\",\"UpstreamHttpMethod\":[\"Get\",\"Post\",\"Put\",\"Patch\",\"Delete\"],\"RateLimitOptions\":{\"ClientWhitelist\":[],\"EnableRateLimiting\":true,\"Period\":\"1s\",\"PeriodTimespan\":1,\"Limit\":10},\"FileCacheOptions\":{\"TtlSeconds\":0,\"Region\":\"Auth_VN\"},\"HttpHandlerOptions\":{\"AllowAutoRedirect\":true,\"UseCookieContainer\":true,\"UseTracing\":true,\"MaxConnectionsPerServer\":20}},{\"Priority\":6,\"DownstreamPathTemplate\":\"/{everything}\",\"DownstreamScheme\":\"https\",\"DownstreamHostAndPorts\":[{\"Host\":\"localhost\",\"Port\":7034}],\"UpstreamPathTemplate\":\"/integration/{everything}\",\"UpstreamHttpMethod\":[\"Get\",\"Post\",\"Put\",\"Patch\",\"Delete\"],\"RateLimitOptions\":{\"ClientWhitelist\":[],\"EnableRateLimiting\":true,\"Period\":\"1s\",\"PeriodTimespan\":1,\"Limit\":1000},\"FileCacheOptions\":{\"TtlSeconds\":0,\"Region\":\"Integration_VN\"},\"HttpHandlerOptions\":{\"AllowAutoRedirect\":true,\"UseCookieContainer\":true,\"UseTracing\":true,\"MaxConnectionsPerServer\":1000}},{\"Priority\":9,\"DownstreamPathTemplate\":\"/tms/{everything}\",\"DownstreamScheme\":\"https\",\"DownstreamHostAndPorts\":[{\"Host\":\"localhost\",\"Port\":7034}],\"UpstreamPathTemplate\":\"/tms/{everything}\",\"UpstreamHttpMethod\":[\"Get\",\"Post\",\"Put\",\"Patch\",\"Delete\"],\"RateLimitOptions\":{\"ClientWhitelist\":[],\"EnableRateLimiting\":true,\"Period\":\"1s\",\"PeriodTimespan\":1,\"Limit\":1000},\"FileCacheOptions\":{\"TtlSeconds\":0,\"Region\":\"TMS_VN\"},\"HttpHandlerOptions\":{\"AllowAutoRedirect\":true,\"UseCookieContainer\":true,\"UseTracing\":true,\"MaxConnectionsPerServer\":1000}},{\"Priority\":7,\"DownstreamPathTemplate\":\"/{everything}\",\"DownstreamScheme\":\"https\",\"DownstreamHostAndPorts\":[{\"Host\":\"localhost\",\"Port\":7186}],\"UpstreamPathTemplate\":\"/invoice/{everything}\",\"UpstreamHttpMethod\":[\"Get\",\"Post\",\"Put\",\"Patch\",\"Delete\"],\"RateLimitOptions\":{\"ClientWhitelist\":[],\"EnableRateLimiting\":true,\"Period\":\"1s\",\"PeriodTimespan\":1,\"Limit\":1000},\"FileCacheOptions\":{\"TtlSeconds\":0,\"Region\":\"Invoice_VN\"},\"HttpHandlerOptions\":{\"AllowAutoRedirect\":true,\"UseCookieContainer\":true,\"UseTracing\":true,\"MaxConnectionsPerServer\":1000}},{\"Priority\":8,\"DownstreamPathTemplate\":\"/{everything}\",\"DownstreamScheme\":\"https\",\"DownstreamHostAndPorts\":[{\"Host\":\"localhost\",\"Port\":7015}],\"UpstreamPathTemplate\":\"/one/{everything}\",\"UpstreamHttpMethod\":[\"Get\",\"Post\",\"Put\",\"Patch\",\"Delete\"],\"RateLimitOptions\":{\"ClientWhitelist\":[],\"EnableRateLimiting\":true,\"Period\":\"1s\",\"PeriodTimespan\":1,\"Limit\":1000},\"FileCacheOptions\":{\"TtlSeconds\":0,\"Region\":\"One_VN\"},\"HttpHandlerOptions\":{\"AllowAutoRedirect\":true,\"UseCookieContainer\":true,\"UseTracing\":true,\"MaxConnectionsPerServer\":1000}},{\"Priority\":2,\"DownstreamPathTemplate\":\"/bms/{everything}\",\"DownstreamScheme\":\"https\",\"DownstreamHostAndPorts\":[{\"Host\":\"localhost\",\"Port\":7080}],\"UpstreamPathTemplate\":\"/bms/{everything}\",\"UpstreamHttpMethod\":[\"Get\",\"Post\",\"Put\",\"Patch\",\"Delete\"],\"RateLimitOptions\":{\"ClientWhitelist\":[],\"EnableRateLimiting\":true,\"Period\":\"1s\",\"PeriodTimespan\":1,\"Limit\":1000},\"FileCacheOptions\":{\"TtlSeconds\":120,\"Region\":\"BMS_VN\"},\"HttpHandlerOptions\":{\"AllowAutoRedirect\":true,\"UseCookieContainer\":true,\"UseTracing\":true,\"MaxConnectionsPerServer\":1000}}],\"GlobalConfiguration\":{\"BaseUrl\":\"\"}}", false });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "mix_global_setting");
        }
    }
}
