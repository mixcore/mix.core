namespace Mix.Lib.Interfaces
{
    public interface IMixConfigurationService
    {
        public List<MixConfigurationContentViewModel> Configs { get; }

        public Task Reload(UnitOfWorkInfo<MixCmsContext> uow = null);

        public Task Set(string name, string content, string culture, int cultureId, UnitOfWorkInfo<MixCmsContext> uow);

        public string GetConfig(string name, string culture, string defaultValue = default);
    }
}
