using Mix.Database.Entities.Cms.v2;
using Mix.Heart.Repository;
using Mix.Heart.UnitOfWork;
using Mix.Portal.Domain.Base;
using Newtonsoft.Json.Linq;
using System;

namespace Mix.Portal.Domain.ViewModels
{
    public class MixDataViewModel : SiteDataWithContentViewModelBase<MixCmsContext, MixData, Guid, MixDataContent, MixDataContentViewModel>
    {
        private string databaseName;
        private JObject data;

        #region Properties

        public int MixDatabaseId { get; set; }
        public string MixDatabaseName { get; set; }

        public MixDataContentViewModel SaveDataContent { get; set; }
        #endregion

        #region Contructors

        public MixDataViewModel()
        {

        }

        public MixDataViewModel(Repository<MixCmsContext, MixData, Guid> repository) : base(repository)
        {
        }

        public MixDataViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public MixDataViewModel(string lang, int cultureId, string databaseName, JObject data)
        {
            MixDatabaseName = databaseName;
            this.data = data;
            SaveDataContent = new(lang, cultureId, databaseName, data);
        }

        public MixDataViewModel(MixData entity, UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
        {
        }

        #endregion

        #region Overrides

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
