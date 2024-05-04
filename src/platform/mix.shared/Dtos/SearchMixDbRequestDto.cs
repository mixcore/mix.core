using Microsoft.AspNetCore.Http;
using Mix.Heart.Enums;

namespace Mix.Shared.Dtos
{
    public class SearchMixDbRequestDto : SearchRequestDto
    {
        public SearchMixDbRequestDto()
        {

        }
        public SearchMixDbRequestDto(SearchRequestDto req, HttpRequest request)
        {
            Culture = req.Culture;
            Keyword = req.Keyword;
            FromDate = req.FromDate;
            ToDate = req.ToDate;
            PageIndex = req.PageIndex;
            PageSize = req.PageSize;
            OrderBy = req.OrderBy;
            Direction = req.Direction;
            Status = req.Status;
        }

        public List<SearchQueryField> Queries { get; set; } = new();
        public string? ParentId { get; set; }
        public object? ObjParentId => GetParentId();
        public MixDatabaseRelationshipType? Relationship { get; set; } = MixDatabaseRelationshipType.OneToMany;
        public string ParentName { get; set; } = default;

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
    }
}
