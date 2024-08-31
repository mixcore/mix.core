namespace Mix.Database.Entities.MixDb
{
    public sealed class MixPermission : EntityBase<int>
    {
        public int MixTenantId { get; set; }
        public string DisplayName { get; set; }
        public string Group { get; set; }
        public string Key { get; set; }
    }
    public sealed class Metadata
    {
        public string Description { get; set; }
    }
}
