namespace Mix.Portal.Domain.Interfaces
{
    public interface ICloneCultureService
    {
        public Task CloneDefaultCulture(string srcCulture, string destCulture, CancellationToken cancellationToken = default);
    }
}
