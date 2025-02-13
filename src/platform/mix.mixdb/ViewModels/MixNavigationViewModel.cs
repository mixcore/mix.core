using Mix.Database.Constants;
using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;

namespace Mix.Mixdb.ViewModels
{
    public class MixNavigationViewModel : ViewModelBase<MixDbDbContext, MixNavigation, int, MixNavigationViewModel>
    {
        #region Properties
        public string Type { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string Image { get; set; }
        public int TenantId { get; set; }

        public List<MixMenuItemViewModel> MenuItems { get; set; }
        #endregion

        #region Constructors
        public MixNavigationViewModel()
        {
        }

        public MixNavigationViewModel(MixDbDbContext context) : base(context)
        {
        }

        public MixNavigationViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public MixNavigationViewModel(MixNavigation entity, UnitOfWorkInfo uowInfo) : base(entity, uowInfo)
        {
        }
        #endregion

        #region Overrides

        public override async Task ExpandView(CancellationToken cancellationToken = default)
        {
            var menuIds = Context.MixDatabaseAssociation.Where(
                            m =>
                            m.TenantId == TenantId && !m.IsDeleted
                            && m.ParentDatabaseName == MixDbDatabaseNames.Navigation
                                && m.ChildDatabaseName == MixDbDatabaseNames.MenuItem
                                && m.ParentId == Id).Select(m => m.ChildId);
            MenuItems = await MixMenuItemViewModel.GetRepository(UowInfo, CacheService).GetListAsync(m => m.TenantId == TenantId
                                                && !m.IsDeleted && menuIds.Contains(m.Id));
        }

        #endregion
    }
}