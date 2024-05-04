namespace Mix.Database.Entities.MixDb
{
    public class MixMetadataContentAssociation : EntityBase<int>
    {
        public MixContentType? ContentType { get; set; }
        public int ContentId { get; set; }
        public int MetadataId { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int MixTenantId { get; set; }
    }
}
