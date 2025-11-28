using Mix.Constant.Enums;
using Mix.Database.Entities.Cms;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Heart.Enums;
using Mix.Heart.Extensions;
using Mix.Heart.Helpers;
using Mix.Heart.UnitOfWork;
using Mix.Mixdb.Base;
using System.Text.Json.Serialization;

namespace Mix.RepoDb.ViewModels
{
    public sealed class MixDbDatabaseReadViewModel
        : TenantDataViewModelBase<MixCmsContext, MixDbDatabase, int, MixDbDatabaseReadViewModel>
    {
        #region Properties
        public MixDatabaseNamingConvention NamingConvention { get; set; }
        public MixDatabaseProvider DatabaseProvider { get; set; }
        public string ConnectionString { get; set; }
        public string Schema { get; set; }
        public string SystemName { get; set; }
        [JsonIgnore]
        public string DecryptedConnectionString { get; set; }
        #endregion

        #region Constructors

        public MixDbDatabaseReadViewModel()
        {

        }

        public MixDbDatabaseReadViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public MixDbDatabaseReadViewModel(MixDbDatabase entity, UnitOfWorkInfo uowInfo)
            : base(entity, uowInfo)
        {
        }

        #endregion

        #region Overrides

        #endregion
    }
}
