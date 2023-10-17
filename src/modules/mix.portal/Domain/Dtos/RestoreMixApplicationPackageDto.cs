namespace Mix.Portal.Domain.Dtos
{
    public sealed class RestoreMixApplicationPackageDto
    {
        public int AppId { get; set; }
        public string PackageFilePath { get; set; }
    }
}
