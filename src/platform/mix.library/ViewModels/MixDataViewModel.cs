namespace Mix.Lib.ViewModels
{
    public sealed class MixDataViewModel
        : SiteDataWithContentViewModelBase<MixCmsContext, MixData, Guid, MixDataViewModel, MixDataContent, MixDataContentViewModel>
    {
        #region Properties

        public int MixDatabaseId { get; set; }
        public string MixDatabaseName { get; set; }

        public MixDataContentViewModel SaveDataContent { get; set; }
        #endregion

        #region Constructors

        public MixDataViewModel()
        {

        }

        public MixDataViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public MixDataViewModel(string lang, int cultureId, string databaseName, JObject data)
        {
            MixDatabaseName = databaseName;
            SaveDataContent = new(lang, cultureId, databaseName, data);
        }

        public MixDataViewModel(MixData entity, UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
        {
        }

        #endregion

        #region Overrides

        public override void InitDefaultValues(string language = null, int? domainId = null)
        {
            base.InitDefaultValues(language, domainId);
        }

        private void ParseData(JObject data)
        {

        }

        #endregion
    }
}
