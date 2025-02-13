using Microsoft.AspNetCore.Http;
using Mix.Heart.Enums;
using Mix.Heart.Model;

namespace Mix.Shared.Dtos
{
    public class SearchMixDbRequestDto
    {
        public SearchMixDbRequestDto()
        {

        }
        public SearchMixDbRequestDto(SearchRequestDto req, HttpRequest request)
        {
            PageIndex = req.PageIndex;
            PageSize = req.PageSize;
            SortBy = req.SortBy ?? req.OrderBy;
            Direction = req.Direction;
            Status = req.Status;
        }
        public MixContentStatus? Status { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public object? After { get; set; }
        public object? Before { get; set; }
        public int PageIndex { get; set; }
        public int? PageSize { get; set; }
        public string? PagingState { get; set; }
        public string? SortBy { get; set; }
        public SortDirection Direction { get; set; }
        public string? MixDatabaseName { get; set; }
        public MixConjunction Conjunction { get; set; } = MixConjunction.And;
        public string? SelectColumns { get; set; }
        public string? ParentId { get; set; }
        public object? ObjParentId => GetParentId();
        public MixDatabaseRelationshipType? Relationship { get; set; } = MixDatabaseRelationshipType.OneToMany;
        public string? ParentName { get; set; }
        public List<MixQueryField>? Queries { get; set; }
        public List<MixSortByColumn>? SortByColumns { get; set; }

        public List<SearchMixDbRequestDto> RelatedDataRequests { get; set; }

        #region Helpers

        private object? GetParentId()
        {
            try
            {
                if (Guid.TryParse(ParentId, out var guidId))
                {
                    return guidId;
                }
                else
                {
                    if (int.TryParse(ParentId, out var intId))
                    {
                        return intId;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.Badrequest, ex);
            }
        }
        #endregion
    }
}
