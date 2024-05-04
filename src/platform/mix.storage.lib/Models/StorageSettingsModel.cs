﻿using Mix.Storage.Lib.Engines.Aws;
using Mix.Storage.Lib.Engines.AzureStorage;
using Mix.Storage.Lib.Engines.CloudFlare;

namespace Mix.Storage.Lib.Models
{
    public sealed class StorageSettingsModel
    {
        public MixStorageProvider Provider { get; set; }
        public bool IsAutoScaleImage { get; set; }
        public List<ImageSize> ImageSizes { get; set; } = new();
        public AzureStorageSettings AzureStorageSetting { get; set; }
        public AwsSetting AwsSetting { get; set; }
        public CloudFlareSettings CloudFlareSetting { get; set; }
    }
}
