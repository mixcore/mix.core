namespace Mix.Lib.Dtos
{
    public class SearchDataContentDto : SearchRequestDto
    {
        public SearchDataContentDto()
        {

        }
        public int? IntParentId { get; set; }
        public Guid? GuidParentId { get; set; }
        public int MixDatabaseId { get; set; }
        public string MixDatabaseName { get; set; }
        public MixCompareOperatorKind CompareKind { get; set; }
        public bool IsGroup { get; set; }

    }
}
