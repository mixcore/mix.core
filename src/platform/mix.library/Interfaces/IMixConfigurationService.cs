namespace Mix.Lib.Interfaces
{
    public interface IMixConfigurationService
    {
        public List<MixConfigurationContentViewModel> Configs { get; }

        public Task Reload(int tenantId, UnitOfWorkInfo<MixCmsContext> uow = null);

        public Task Set(string name, string content, string culture, int cultureId, UnitOfWorkInfo<MixCmsContext> uow, int tenantId);

        public Task<T> GetConfig<T>(string name, string culture, int tenantId, T defaultValue = default);
    }
}
