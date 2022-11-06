namespace Mix.Database.Entities.Cms
{
    public class MixUrlAlias : TenantEntityBase<int>
    {
        public int? SourceContentId { get; set; }

        public Guid? SourceContentGuidId { get; set; }

        public string Alias { get; set; }

        public MixUrlAliasType Type { get; set; }
    }
}
