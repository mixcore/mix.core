using Microsoft.EntityFrameworkCore;
using Mix.Lib.Services;

namespace Mix.Lib.ViewModels
{
    public sealed class MixTenantSystemViewModel
        : ViewModelBase<MixCmsContext, MixTenant, int, MixTenantSystemViewModel>
    {
        #region Properties

        public string PrimaryDomain { get; set; }
        public string SystemName { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }

        public List<MixDomainViewModel> Domains { get; set; } = new();
        public List<MixCulture> Cultures { get; set; } = new();

        public TenantConfigurationModel Configurations { get; set; }
        #endregion

        #region Constructors

        public MixTenantSystemViewModel()
        {
        }

        public MixTenantSystemViewModel(MixTenant entity,

            UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
        {
        }

        public MixTenantSystemViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        #endregion

        #region Overrides

        public override async Task ExpandView(CancellationToken cancellationToken = default)
        {
            Domains = await MixDomainViewModel.GetRepository(UowInfo, CacheService).GetAllAsync(m => m.TenantId == Id, cancellationToken);
            Cultures = await Context.MixCulture.Where(m => m.TenantId == Id).ToListAsync(cancellationToken: cancellationToken);
            Configurations = new TenantConfigurationModel()
            {
                Domain = Domains?.FirstOrDefault()?.Host,
                DefaultCulture = Cultures.First().Specificulture
            };
        }


        #endregion

        #region Methods
        #endregion
    }
}
