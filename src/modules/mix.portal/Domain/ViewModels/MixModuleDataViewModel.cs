namespace Mix.Portal.Domain.ViewModels
{
    public class MixModuleDataViewModel
        : MultilingualSEOContentViewModelBase
            <MixCmsContext, MixModuleData, int, MixModuleDataViewModel>
    {
        #region Contructors

        public MixModuleDataViewModel()
        {
        }

        public MixModuleDataViewModel(MixModuleData entity,

            UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
        {
        }

        public MixModuleDataViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }
        #endregion

        #region Properties
        public string SimpleDataColumns { get; set; }
        public string Value { get; set; }
        public JObject Data { get; set; } = new JObject();
        public List<ModuleColumnModel> Columns { get; set; }
        #endregion

        #region Overrides

        public override Task<MixModuleData> ParseEntity()
        {
            Value = JsonConvert.SerializeObject(Data);
            SimpleDataColumns = JsonConvert.SerializeObject(Columns);
            LastModified = DateTime.Now;
            return base.ParseEntity();
        }

        public override Task ExpandView()
        {
            if (!string.IsNullOrEmpty(SimpleDataColumns))
            {
                JArray arrField = JArray.Parse(SimpleDataColumns);
                Columns = arrField.ToObject<List<ModuleColumnModel>>();
            }
            else
            {
                Columns = new List<ModuleColumnModel>();
            }
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
            return Data.Property(name)?.Value<string>();
        }
        #endregion
    }
}
