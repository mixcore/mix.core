using System.Text.Json.Serialization;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Lib.ViewModels.ReadOnly;

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

        public List<MixDatabaseReadViewModel> Databases { get; set; }

        public string DecryptedConnectionString { get; set; }
        #endregion

        #region Constructors

        public MixDatabaseContextViewModel()
        {
            CacheExpiration = TimeSpan.FromMicroseconds(1);
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
            Databases = await MixDatabaseReadViewModel.GetRepository(UowInfo, CacheService).GetListAsync(m => m.MixDatabaseContextId == Id, cancellationToken);
        }

        public override Task<MixDatabaseContext> ParseEntity(CancellationToken cancellationToken = default)
        {
            return base.ParseEntity(cancellationToken);
        }

        protected override async Task DeleteHandlerAsync(CancellationToken cancellationToken = default)
        {
            var databases = Context.MixDatabase.Where(m => m.MixDatabaseContextId == Id).ToList();
            foreach (var db in databases)
            {
                var cols = Context.MixDatabaseColumn.Where(m => m.MixDatabaseId == db.Id).ToList();
                Context.MixDatabaseColumn.RemoveRange(cols);
                
                var rels = Context.MixDatabaseRelationship.Where(m => m.SourceDatabaseName == db.SystemName || m.DestinateDatabaseName == db.SystemName).ToList();
                Context.MixDatabaseRelationship.RemoveRange(rels);
            }
            Context.MixDatabase.RemoveRange(databases);

            await Context.SaveChangesAsync(cancellationToken);
            await base.DeleteHandlerAsync(cancellationToken);
        }

        #endregion
    }
}
