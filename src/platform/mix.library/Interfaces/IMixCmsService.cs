namespace Mix.Lib.Interfaces
{
    public interface IMixCmsService
    {
        public string GetAssetFolder(string culture, string domain);
        public MixTenantSystemModel GetCurrentTenant();
        Task<FileModel> ParseSitemapAsync(CancellationToken cancellationToken = default);
    }
}