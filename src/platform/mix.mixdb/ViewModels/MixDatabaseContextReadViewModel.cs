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
    public sealed class MixDatabaseContextReadViewModel
        : TenantDataViewModelBase<MixCmsContext, MixDatabaseContext, int, MixDatabaseContextReadViewModel>
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

        public MixDatabaseContextReadViewModel()
        {

        }

        public MixDatabaseContextReadViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public MixDatabaseContextReadViewModel(MixDatabaseContext entity, UnitOfWorkInfo uowInfo)
            : base(entity, uowInfo)
        {
        }

        #endregion

        #region Overrides

        #endregion
    }
}
