namespace Mixcore.Domain.ViewModels
{
    public class ModuleDataViewModel
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

        public JObject Data { get; set; } = new JObject();
        #endregion

        #region Overrides

        public override Task ExpandView()
        {
            if (!string.IsNullOrEmpty(Value))
            {
                Data = JObject.Parse(Value);
            }
            return base.ExpandView();
        }

        #endregion

        #region Helper
        public string Property(string name)
        {
            var prop = Data.Property(name)?.Value;
            return prop?.Value<string>("value");
        }
        #endregion
    }
}
