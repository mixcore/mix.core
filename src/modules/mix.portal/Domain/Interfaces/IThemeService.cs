namespace Mix.Portal.Domain.Interfaces
{
    public interface IThemeService
    {
        public Task<MixTheme> GetActiveTheme();
    }
}
