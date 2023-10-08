namespace Mix.Lib.ViewModels
{
    public sealed class MixLanguageContentViewModel
        : HaveParentContentViewModelBase<MixCmsContext, MixLanguageContent, int, MixLanguageContentViewModel>
    {
        public string DisplayName { get; set; }
        public string SystemName { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string DefaultContent { get; set; }
        public string Category { get; set; }
        public MixDataType DataType { get; set; }
        public MixLanguageContentViewModel()
        {

        }

        public MixLanguageContentViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public MixLanguageContentViewModel(MixLanguageContent entity, UnitOfWorkInfo uowInfo)
            : base(entity, uowInfo)
        {
        }

        #region Overrides

        public override async Task<int> CreateParentAsync(CancellationToken cancellationToken = default)
        {
            MixLanguageViewModel parent = new(UowInfo)
            {
                SystemName = SystemName,
                DisplayName = DisplayName,
                Description = Description,
                MixTenantId = MixTenantId
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
