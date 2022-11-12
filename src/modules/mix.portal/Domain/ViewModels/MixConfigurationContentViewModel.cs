namespace Mix.Portal.Domain.ViewModels
{
    public sealed class MixConfigurationContentViewModel
        : MultilingualUniqueNameContentViewModelBase<MixCmsContext, MixConfigurationContent, int, MixConfigurationContentViewModel>
    {
        public string DefaultContent { get; set; }
        public string Category { get; set; }
        public MixDataType DataType { get; set; }
        public MixConfigurationContentViewModel()
        {

        }

        public MixConfigurationContentViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public MixConfigurationContentViewModel(MixConfigurationContent entity,
            UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
        {
        }

        public T GetValue<T>()
        {
            try
            {
                return System.Text.Json.JsonSerializer.Deserialize<T>(Content);
            }
            catch { return default(T); }
        }
    }
}
