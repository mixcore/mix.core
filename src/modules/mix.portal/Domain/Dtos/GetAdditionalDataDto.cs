namespace Mix.Portal.Domain.Dtos
{
    public class GetAdditionalDataDto
    {
        public string Specificulture { get; set; }
        public string ParentId { get; set; }

        public MixDatabaseParentType? ParentType { get; set; }
        public string DatabaseName { get; set; }
        public Guid? GuidParentId
        {
            get
            {
                return Guid.TryParse(ParentId, out var guidId)
                ? guidId
                : null;
            }
        }
        public int? IntParentId
        {
            get
            {
                return int.TryParse(ParentId, out var intId)
                ? intId : null;
            }
        }

        public bool IsValid()
        {
            return (GuidParentId.HasValue || IntParentId.HasValue) && ParentType.HasValue;
        }
    }
}
