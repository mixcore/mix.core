namespace Mix.Lib.Interfaces
{
    public interface IMixEdmService
    {
        public Task<string?> GetEdmTemplate(string filename);

        public Task SendMailWithEdmTemplate(string subject, string templateName, JObject data, string to, string? from = null);
    }
}
