using Mix.Heart.Enums;
using Mix.Shared.Enums;
using System;

namespace Mix.Shared.Models
{
    public class GlobalConfigurations
    {
        public ConnectionStrings ConnectionStrings { get; set; }
        public bool IsInit { get; set; }
        public bool ClearDbPool { get; set; }
        public bool IsMaintenance { get; set; }
        public bool IsHttps { get; set; }
        public int? MaxPageSize { get; set; }
        public InitStep InitStatus { get; set; }
        public string DefaultCulture { get; set; }
        public string Domain { get; set; }
        public string ApiEncryptKey { get; set; }
        public MixDatabaseProvider DatabaseProvider { get; set; }
        public SmtpConfiguration Smtp { get; set; }
        public DateTime? LastUpdateConfiguration{ get; set; }
    }

    public class ConnectionStrings
    {
        public string MixAccountConnection { get; set; }
        public string MixCmsConnection { get; set; }
    }

    public class SmtpConfiguration
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public bool SSL { get; set; }
        public string User { get; set; }
    }
}
