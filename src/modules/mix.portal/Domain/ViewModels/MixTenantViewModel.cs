namespace Mix.Portal.Domain.ViewModels
{
    [GeneratePublisher]
    public sealed class MixTenantViewModel
        : ViewModelBase<MixCmsContext, MixTenant, int, MixTenantViewModel>
    {
        #region Properties

        public string PrimaryDomain { get; set; }
        public string SystemName { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }

        public MixCultureViewModel Culture { get; set; }
        public List<MixDomainViewModel> Domains { get; set; } = new();
        #endregion

        #region Constructors

        public MixTenantViewModel()
        {
        }

        public MixTenantViewModel(MixTenant entity,
            UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
        {
        }

        public MixTenantViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        #endregion

        #region Overrides

        public override async Task ExpandView(CancellationToken cancellationToken = default)
        {
            Domains = await MixDomainViewModel.GetRepository(UowInfo, CacheService).GetAllAsync(m => m.MixTenantId == Id, cancellationToken);
        }

        public override async Task Validate(CancellationToken cancellationToken)
        {
            IsValid = IsValid && !Context.MixDomain.Any(m => m.Host == PrimaryDomain && m.MixTenantId != Id);
            if (!IsValid)
            {
                Errors.Add(new ValidationResult($"{PrimaryDomain} is used, please try another one"));
                await HandleErrorsAsync();
            }
            await base.Validate(cancellationToken);
        }

        protected override async Task SaveEntityRelationshipAsync(MixTenant parent, CancellationToken cancellationToken = default)
        {
            if (Culture != null)
            {
                await SaveCultureAsync(parent, cancellationToken);
            }

            if (Domains != null)
            {
                await SaveDomainAsync(parent, cancellationToken);
            }
        }

        private async Task SaveCultureAsync(MixTenant parent, CancellationToken cancellationToken = default)
        {
            Culture.SetCacheService(CacheService);
            Culture.MixTenantId = parent.Id;
            Culture.SetUowInfo(UowInfo, CacheService);
            await Culture.SaveAsync(cancellationToken);
        }

        private async Task SaveDomainAsync(MixTenant parent, CancellationToken cancellationToken = default)
        {
            foreach (var domain in Domains)
            {
                domain.MixTenantId = parent.Id;
                domain.SetCacheService(CacheService);
                domain.SetUowInfo(UowInfo, CacheService);
                await domain.SaveAsync(cancellationToken);
            }
        }
        #endregion

        #region Methods

        public void InitDomain()
        {
            Domains = new()
            {
                new()
                {
                    DisplayName = PrimaryDomain,
                    Host = PrimaryDomain,
                    CreatedBy = CreatedBy
                }
            };
        }

        public void CloneCulture(MixCulture culture)
        {
            culture.Id = 0;
            Culture = new MixCultureViewModel(culture, UowInfo);
        }

        #endregion
    }
}
