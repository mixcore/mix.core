namespace Mix.Lib.ViewModels
{
    public sealed class MixDatabaseContextViewModel
        : TenantDataViewModelBase<MixCmsContext, MixDatabaseContext, int, MixDatabaseContextViewModel>
    {
        #region Properties
        public MixDatabaseProvider DatabaseProvider { get; set; }
        public string ConnectionString { get; set; }
        public string Schema { get; set; }
        public string SystemName { get; set; }

        public List<MixDatabaseViewModel> Databases { get; set; } = new();
        #endregion

        #region Constructors

        public MixDatabaseContextViewModel()
        {

        }

        public MixDatabaseContextViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public MixDatabaseContextViewModel(MixDatabaseContext entity, UnitOfWorkInfo uowInfo = null)
            : base(entity, uowInfo)
        {
        }

        #endregion

        #region Overrides
        public override async Task ExpandView(CancellationToken cancellationToken = default)
        {
            var dbRepo = MixDatabaseViewModel.GetRepository(UowInfo, CacheService);
            Databases = await dbRepo.GetListAsync(m => m.MixDatabaseContextId == Id, cancellationToken);
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
