namespace Mix.Lib.Interfaces
{
    public interface IMixEdmService
    {
        public void SetTenantId(int tenantId);
        public void SetTenant(MixTenantSystemModel tenant);
        public Task<string> GetEdmTemplate(string filename);
        public Task SendMailWithEdmTemplate(string subject, string templateName, JObject data, string to, string cc = null, string from = null);
    }
}
