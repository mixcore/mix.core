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
