using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Mix.Heart.Helpers;
using Mix.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Shared.Models
{
    public class SearchMixDbRequestModel
    {
        public SearchMixDbRequestModel()
        {
            Paging = new PagingRequestModel();
        }

        public SearchMixDbRequestModel(SearchMixDbRequestDto request)
        {
            Paging = new PagingRequestModel(request);
            TableName = request.MixDatabaseName;
            SelectColumns = request.SelectColumns;
            Conjunction = request.Conjunction;
        }

        public SearchMixDbRequestModel(
            string tableName,
            IEnumerable<MixQueryField> queries,
            PagingRequestModel paging,
            string? requestedBy = default)
        {
            TableName = tableName;
            Queries = queries.ToList();
            Paging = paging;
            RequestedBy = requestedBy;
        }

        public SearchMixDbRequestModel Clone()
        {
            var result = ReflectionHelper.CloneObject(this);
            result.Queries = Queries.ToList();
            return result;
        }

        public string? SelectColumns { get; set; }
        public MixConjunction Conjunction { get; set; } = MixConjunction.And;
        public string TableName { get; set; }
        public string? RequestedBy { get; set; }
        public List<MixQueryField> Queries { get; set; }
        public PagingRequestModel Paging { get; set; }
        public List<SearchMixDbRequestModel> RelatedDataRequests { get; set; }
    }
}
