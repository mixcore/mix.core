using System.Text.Json.Serialization;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Lib.ViewModels.ReadOnly;

namespace Mix.Lib.ViewModels
{
    public sealed class MixDatabaseContextViewModel
        : TenantDataViewModelBase<MixCmsContext, MixDbDatabase, int, MixDatabaseContextViewModel>
    {
        #region Properties
        public MixDatabaseProvider DatabaseProvider { get; set; }
        public MixDatabaseNamingConvention NamingConvention { get; set; }
        public string ConnectionString { get; set; }
        public string Schema { get; set; }
        public string SystemName { get; set; }

        public List<MixDbTableReadViewModel> Databases { get; set; }

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

        public MixDatabaseContextViewModel(MixDbDatabase entity, UnitOfWorkInfo uowInfo)
            : base(entity, uowInfo)
        {
        }

        #endregion

        #region Overrides
        public override async Task ExpandView(CancellationToken cancellationToken = default)
        {
            Databases = await MixDbTableReadViewModel.GetRepository(UowInfo, CacheService).GetListAsync(m => m.MixDbTableId == Id, cancellationToken);
        }

        public override Task<MixDbDatabase> ParseEntity(CancellationToken cancellationToken = default)
        {
            return base.ParseEntity(cancellationToken);
        }

        protected override async Task DeleteHandlerAsync(CancellationToken cancellationToken = default)
        {
            var databases = Context.MixDbTable.Where(m => m.MixDbTableId == Id).ToList();
            foreach (var db in databases)
            {
                var cols = Context.MixDbColumn.Where(m => m.MixDbTableId == db.Id).ToList();
                Context.MixDbColumn.RemoveRange(cols);
                
                var rels = Context.MixDbTableRelationship.Where(m => m.SourceDatabaseName == db.SystemName || m.DestinateDatabaseName == db.SystemName).ToList();
                Context.MixDbTableRelationship.RemoveRange(rels);
            }
            Context.MixDbTable.RemoveRange(databases);

            await Context.SaveChangesAsync(cancellationToken);
            await base.DeleteHandlerAsync(cancellationToken);
        }

        #endregion
    }
}
