﻿using Microsoft.AspNetCore.Http;

namespace Mix.Lib.Models.Common
{
    public class SearchDataContentAssociationModel : SearchQueryModel<MixDataContentAssociation, Guid>
    {
        public SearchDataContentAssociationModel(int tenantId)
            : base(tenantId)
        {

        }
        public SearchDataContentAssociationModel(int tenantId, SearchRequestDto req, HttpRequest httpRequest)
            : base(tenantId, req, httpRequest)
        {
            Keyword = req.Keyword;
            FromDate = req.FromDate;
            ToDate = req.ToDate;
            Status = req.Status;
            MixDatabaseName = httpRequest.Query[MixRequestQueryKeywords.DatabaseName];

            if (int.TryParse(httpRequest.Query[MixRequestQueryKeywords.DatabaseId], out int mixDbId))
            {
                MixDatabaseId = mixDbId;
            }
            if (int.TryParse(httpRequest.Query[MixRequestQueryKeywords.IntParentId], out int intParentId))
            {
                IntParentId = intParentId;
            }
            if (Guid.TryParse(httpRequest.Query[MixRequestQueryKeywords.GuidParentId], out Guid guidParentId))
            {
                GuidParentId = guidParentId;
            }
            if (bool.TryParse(httpRequest.Query["isGroup"], out bool isGroup))
            {
                IsGroup = isGroup;
            }
            if (Enum.TryParse(httpRequest.Query["compareKind"], out MixCompareOperatorKind compareKind))
            {
                CompareKind = compareKind;
            }
            if (!string.IsNullOrEmpty(httpRequest.Query["fields"]))
            {
                Fields = JObject.Parse(httpRequest.Query["fields"]);
            }
        }
        public string ParentId { get; set; }
        public int? IntParentId { get; set; }
        public Guid? GuidParentId { get; set; }
        public int MixDatabaseId { get; set; }
        public string MixDatabaseName { get; set; }
        public MixCompareOperatorKind CompareKind { get; set; }
        public bool IsGroup { get; set; }
        public JObject Fields { get; set; }


        protected override void BuildAndPredicate(SearchRequestDto req, HttpRequest request)
        {
            SearchDataContentModel searchReq = new SearchDataContentModel(MixTenantId, req, request);
            base.BuildAndPredicate(req, request);
            AndPredicate = AndPredicate.AndAlso(m =>
            m.MixDatabaseId == searchReq.MixDatabaseId
            || m.MixDatabaseName == searchReq.MixDatabaseName);
            AndPredicate = AndPredicate.AndAlsoIf(searchReq.IntParentId.HasValue, m => m.IntParentId == searchReq.IntParentId);
            AndPredicate = AndPredicate.AndAlsoIf(searchReq.GuidParentId.HasValue, m => m.GuidParentId == searchReq.GuidParentId);
        }
    }
}