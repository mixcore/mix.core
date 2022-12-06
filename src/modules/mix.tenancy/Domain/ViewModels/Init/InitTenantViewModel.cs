using Mix.Lib.Services;
using Mix.Tenancy.Domain.Dtos;

namespace Mix.Tenancy.Domain.ViewModels.Init
{
    public class InitTenantViewModel : ViewModelBase<MixCmsContext, MixTenant, int, InitTenantViewModel>
    {
        public string PrimaryDomain { get; set; }
        public string SystemName { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }

        public InitCultureViewModel Culture { get; set; }
        public InitDomainViewModel Domain { get; set; }

        public InitTenantViewModel() : base()
        {
        }

        public InitTenantViewModel(MixTenant entity,

            UnitOfWorkInfo uowInfo = null)
            : base(entity, uowInfo)
        {
        }

        public InitTenantViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public InitTenantViewModel(MixCmsContext context) : base(context)
        {
        }

        public InitTenantViewModel(MixCmsContext context, InitCmsDto model) : base(context)
        {
            PrimaryDomain = model.PrimaryDomain;
            DisplayName = model.SiteName;
            SystemName = model.SiteName.ToSEOString('_');
            Description = model.SiteName;
            Status = MixContentStatus.Published;
            CreatedDateTime = DateTime.UtcNow;

            InitCulture(model);

            InitDomain(model);
        }

        private void InitDomain(InitCmsDto model)
        {
            Domain = new(UowInfo)
            {
                DisplayName = model.PrimaryDomain,
                Host = model.PrimaryDomain,
                CreatedBy = CreatedBy
            };
        }

        private void InitCulture(InitCmsDto model)
        {
            Culture = new InitCultureViewModel(UowInfo)
            {
                Title = model.Culture.FullName,
                Specificulture = model.Culture.Specificulture,
                Alias = model.Culture.Alias,
                Icon = model.Culture.Icon,
                DisplayName = model.Culture.FullName,
                SystemName = model.Culture.Specificulture,
                Description = model.SiteName,
                Status = MixContentStatus.Published,
                CreatedDateTime = DateTime.UtcNow,
                CreatedBy = CreatedBy
            };
        }

        protected override async Task<MixTenant> SaveHandlerAsync(CancellationToken cancellationToken = default)
        {
            var entity = await base.SaveHandlerAsync(cancellationToken);
            var tenantConfigService = new TenantConfigService(SystemName);
            tenantConfigService.AppSettings.DefaultCulture = Culture.Specificulture;
            tenantConfigService.AppSettings.Domain = PrimaryDomain.TrimEnd('/');
            tenantConfigService.AppSettings.ApiEncryptKey = AesEncryptionHelper.GenerateCombinedKeys();
            tenantConfigService.SaveSettings();
            return entity;
        }

        protected override async Task SaveEntityRelationshipAsync(MixTenant parent, CancellationToken cancellationToken = default)
        {
            if (Culture != null)
            {
                await SaveCultureAsync(parent, cancellationToken);
            }

            if (Domain != null)
            {
                await SaveDomainAsync(parent);
            }
        }

        private async Task SaveCultureAsync(MixTenant parent, CancellationToken cancellationToken = default)
        {
            Culture.MixTenantId = parent.Id;
            Culture.SetUowInfo(UowInfo);
            await Culture.SaveAsync(cancellationToken);
        }

        private async Task SaveDomainAsync(MixTenant parent)
        {
            Domain.MixTenantId = parent.Id;
            Domain.SetUowInfo(UowInfo);
            await Domain.SaveAsync();
        }

        public override void InitDefaultValues(string language = null, int? cultureId = null)
        {
            CreatedDateTime = DateTime.UtcNow;
            Status = MixContentStatus.Published;
        }
    }
}
