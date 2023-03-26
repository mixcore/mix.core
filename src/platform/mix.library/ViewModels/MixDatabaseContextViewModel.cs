namespace Mix.Lib.ViewModels
{
    [GenerateRestApiController]
    public sealed class MixDatabaseContextViewModel
        : TenantDataViewModelBase<MixCmsContext, MixDatabaseContext, int, MixDatabaseContextViewModel>
    {
        #region Properties
        public MixDatabaseProvider DatabaseProvider { get; set; }
        public string ConnectionString { get; set; }

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
            var associations = Context.MixDatabaseContextDatabaseAssociation.Where(m => m.ParentId == Id).Select(m => m.ChildId);
            Databases = await dbRepo.GetListAsync(m => associations.Any(a => a == m.Id), cancellationToken);
        }

        protected override async Task DeleteHandlerAsync(CancellationToken cancellationToken = default)
        {
            var associations = Context.MixDatabaseContextDatabaseAssociation
                .Where(m => m.ParentId == Id);

            Context.MixDatabaseContextDatabaseAssociation.RemoveRange(associations);

            await Context.SaveChangesAsync(cancellationToken);
            await base.DeleteHandlerAsync(cancellationToken);
        }

        #endregion
    }
}
