using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Storage.Lib.Engines.AzureStorage
{
    public sealed class AzureStorageSettings
    {
        public string StorageAccountName { get; set; }
        public string AzureWebJobStorage { get; set; }
        public string ContainerName { get; set; }
        public string CdnUrl { get; set; }

    }
}
