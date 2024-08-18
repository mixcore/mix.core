using System.Text.Json.Serialization;

namespace Mix.Lib.ViewModels
{
    public sealed class MixDatabaseContextViewModel
        : TenantDataViewModelBase<MixCmsContext, MixDatabaseContext, int, MixDatabaseContextViewModel>
    {
        #region Properties
        public MixDatabaseProvider DatabaseProvider { get; set; }
        public MixDatabaseNamingConvention NamingConvention { get; set; }
        public string ConnectionString { get; set; }
        public string Schema { get; set; }
        public string SystemName { get; set; }

        public List<MixDatabaseViewModel> Databases { get; set; } = new();

        [JsonIgnore]
        public string DecryptedConnectionString { get; set; }
        #endregion

        #region Constructors

        public MixDatabaseContextViewModel()
        {

        }

        public MixDatabaseContextViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public MixDatabaseContextViewModel(MixDatabaseContext entity, UnitOfWorkInfo uowInfo)
            : base(entity, uowInfo)
        {
        }

        #endregion

        #region Overrides
        public override async Task ExpandView(CancellationToken cancellationToken = default)
        {
            Databases = await MixDatabaseViewModel.GetRepository(UowInfo, CacheService).GetListAsync(m => m.MixDatabaseContextId == Id, cancellationToken);
            if (ConnectionString.IsBase64())
            {
                DecryptedConnectionString ??= AesEncryptionHelper.DecryptString(ConnectionString, GlobalConfigService.Instance.AppSettings.ApiEncryptKey);
            }
            else
            {
                DecryptedConnectionString = ConnectionString;
            }
        }

        public override Task<MixDatabaseContext> ParseEntity(CancellationToken cancellationToken = default)
        {
            if (!string.IsNullOrEmpty(DecryptedConnectionString))
            {
                ConnectionString = AesEncryptionHelper.EncryptString(DecryptedConnectionString, GlobalConfigService.Instance.AppSettings.ApiEncryptKey);
            }
            return base.ParseEntity(cancellationToken);
        }

        protected override async Task DeleteHandlerAsync(CancellationToken cancellationToken = default)
        {
            var databases = Context.MixDatabase.Where(m => m.MixDatabaseContextId == Id);

            Context.MixDatabase.RemoveRange(databases);

            await Context.SaveChangesAsync(cancellationToken);
            await base.DeleteHandlerAsync(cancellationToken);
        }

        #endregion
    }
}
