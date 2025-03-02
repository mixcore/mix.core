using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;

namespace Mix.Mixdb.ViewModels
{
    public class MixPortalMenuViewModel : ViewModelBase<MixDbDbContext, MixPortalMenu, int, MixPortalMenuViewModel>
    {
        #region Properties
        public string? Title { get; set; }
        public string? Icon { get; set; }
        public string? Svg { get; set; }
        public string? Path { get; set; }
        public string? Role { get; set; }
        public int TenantId { get; set; }
        public int? PortalMenuId { get; set; }

        public List<MixPortalMenuViewModel> SubMenus { get; set; }
        #endregion

        #region Constructors
        public MixPortalMenuViewModel()
        {
        }

        public MixPortalMenuViewModel(MixDbDbContext context) : base(context)
        {
        }

        public MixPortalMenuViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public MixPortalMenuViewModel(MixPortalMenu entity, UnitOfWorkInfo uowInfo) : base(entity, uowInfo)
        {
        }
        #endregion

        #region Overrides

        public override async Task ExpandView(CancellationToken cancellationToken = default)
        {
            SubMenus = await GetRepository(UowInfo, CacheService).GetAllAsync(m => m.PortalMenuId == Id, cancellationToken);
        }

        #endregion
    }
}
