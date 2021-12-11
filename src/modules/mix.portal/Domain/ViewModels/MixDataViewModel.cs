namespace Mix.Portal.Domain.ViewModels
{
    public class MixDataViewModel 
        : SiteDataWithContentViewModelBase<MixCmsContext, MixData, Guid, MixDataViewModel, MixDataContent, MixDataContentViewModel>
    {
        #region Properties

        public int MixDatabaseId { get; set; }
        public string MixDatabaseName { get; set; }

        public MixDataContentViewModel SaveDataContent { get; set; }
        #endregion

        #region Contructors

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

        public MixDataViewModel(MixData entity,
            MixCacheService cacheService = null,
            UnitOfWorkInfo uowInfo = null) : base(entity, cacheService, uowInfo)
        {
        }

        #endregion

        #region Overrides

        public override Task ExpandView(MixCacheService cacheService = null)
        {
            MixDatabaseName ??= MixDatabaseNames.MODULE_COLUMN;
            return base.ExpandView(cacheService);
        }

        public override void InitDefaultValues(string language = null, int? cultureId = null)
        {
            base.InitDefaultValues(language, cultureId);
        }

        private void ParseData(JObject data)
        {
            
        }

        #endregion
    }
}
