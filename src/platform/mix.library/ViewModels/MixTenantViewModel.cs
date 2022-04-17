using System.ComponentModel.DataAnnotations;

namespace Mix.Lib.ViewModels
{
    [GeneratePublisher]
    public class MixTenantViewModel
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

        #region Contructors

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

        public override async Task ExpandView()
        {
            Domains = await MixDomainViewModel.GetRepository(UowInfo).GetAllAsync(m => m.MixTenantId == Id);
        }

        public override async Task Validate()
        {
            await base.Validate();
            if (IsValid)
            {
                IsValid = !Context.MixDomain.Any(m => m.Host == PrimaryDomain && m.MixTenantId != Id);
                if (!IsValid)
                {
                    Errors.Add(new ValidationResult($"{PrimaryDomain} is used, please try another one"));
                    await HandleErrorsAsync();
                }
            }
        }
        protected override async Task SaveEntityRelationshipAsync(MixTenant parent)
        {
            if (Culture != null)
            {
                await SaveCultureAsync(parent);
            }

            if (Domains != null)
            {
                await SaveDomainAsync(parent);
            }
        }

        private async Task SaveCultureAsync(MixTenant parent)
        {
            Culture.MixTenantId = parent.Id;
            Culture.SetUowInfo(UowInfo);
            await Culture.SaveAsync();
        }

        private async Task SaveDomainAsync(MixTenant parent)
        {
            foreach (var domain in Domains)
            {
                domain.MixTenantId = parent.Id;
                domain.SetUowInfo(UowInfo);
                await domain.SaveAsync();
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
