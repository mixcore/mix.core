namespace Mix.Lib.ViewModels
{
    public sealed class MixConfigurationContentViewModel
        : HaveParentContentViewModelBase<MixCmsContext, MixConfigurationContent, int, MixConfigurationContentViewModel>
    {
        public string DisplayName { get; set; }
        public string SystemName { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string DefaultContent { get; set; }
        public string Category { get; set; }
        public MixDataType DataType { get; set; }
        public MixConfigurationContentViewModel()
        {

        }

        public MixConfigurationContentViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public MixConfigurationContentViewModel(MixConfigurationContent entity, UnitOfWorkInfo uowInfo)
            : base(entity, uowInfo)
        {
        }

        #region Overrides

        public override async Task<int> CreateParentAsync(CancellationToken cancellationToken = default)
        {
            MixConfigurationViewModel parent = new(UowInfo)
            {
                SystemName = SystemName,
                DisplayName = DisplayName,
                Description = Description,
                TenantId = TenantId
            };
            return await parent.SaveAsync(cancellationToken);
        }

        #endregion

        public T GetValue<T>()
        {
            try
            {
                return System.Text.Json.JsonSerializer.Deserialize<T>(Content);
            }
            catch { return default; }
        }
    }
}
