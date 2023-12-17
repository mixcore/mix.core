namespace Mixcore.Domain.ViewModels
{
    public sealed class ModuleDataViewModel
        : MultilingualSEOContentViewModelBase
            <MixCmsContext, MixModuleData, int, ModuleDataViewModel>
    {
        #region Constructors

        public ModuleDataViewModel()
        {
        }

        public ModuleDataViewModel(MixModuleData entity,

            UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
        {
        }

        public ModuleDataViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }
        #endregion

        #region Properties
        public string Value { get; set; }

        public JObject Data { get; set; }
        #endregion

        #region Overrides

        public override Task ExpandView(CancellationToken cancellationToken = default)
        {
            if (!string.IsNullOrEmpty(Value) && Data == null)
            {
                Data = [];
                var tmp = JObject.Parse(Value);
                foreach (var prop in tmp.Properties())
                {
                    Data.Add(new JProperty(prop.Name, prop.First().Value<string>("value")));
                }
            }
            return base.ExpandView(cancellationToken);
        }

        #endregion

        #region Helper
        public string Property(string name)
        {
            return Data.Property(name)?.Value.ToString();
        }
        #endregion
    }
}
