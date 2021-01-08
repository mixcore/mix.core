using System;
using System.Collections.Generic;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Constants;
namespace Mix.Cms.Lib.Models.Cms
{
    public partial class Efmigrationshistory
    {
        public string MigrationId { get; set; }
        public string ProductVersion { get; set; }
    }
}
