using Mix.Auth.Models;

namespace Mix.Tenancy.Domain.Dtos
{
    public class InitFullSiteDto
    {
        public InitCmsDto TenantData { get; set; }

        public RegisterRequestModel AccountData { get; set; }

        public InitFullSiteDto() { }
    }
}