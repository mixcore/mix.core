namespace Mix.Portal.Domain.Dtos
{
    public class GetAdditionalDataDto
    {
        public string ParentId { get; set; }
        
        public MixDatabaseParentType? ParentType { get; set; }
        public string DatabaseName { get; set; }
        public Guid? GuidParentId
        {
            get
            {
                Guid.TryParse(ParentId, out var guidId);
                return guidId;
            }
        }
        public int? IntParentId
        {
            get
            {
                int.TryParse(ParentId, out var intId);
                return intId;
            }
        }

        public bool IsValid()
        {
            return (GuidParentId.HasValue || IntParentId.HasValue) && ParentType.HasValue;
        }
    }
}
