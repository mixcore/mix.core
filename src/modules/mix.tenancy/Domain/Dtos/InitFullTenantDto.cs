using Mix.Identity.Models.AccountViewModels;

namespace Mix.Tenancy.Domain.Dtos
{
    public class InitFullSiteDto
    {
        public InitCmsDto TenantData { get; set; }

        public RegisterViewModel AccountData { get; set; }

        public InitFullSiteDto() { }
    }
}