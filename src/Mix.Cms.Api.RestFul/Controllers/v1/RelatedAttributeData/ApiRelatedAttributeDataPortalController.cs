// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Mvc;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Controllers;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.ViewModels.MixRelatedAttributeDatas;
using Mix.Domain.Core.ViewModels;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mix.Cms.Api.RestFul.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1/rest/{culture}/related-attribute-data/portal")]
    public class ApiRelatedAttributeDataPortalController :
        BaseAuthorizedRestApiController<MixCmsContext, MixRelatedAttributeData, FormViewModel, FormViewModel, DeleteViewModel>
    {
        // GET: api/v1/rest/{culture}/related-attribute-data
        [HttpGet]
        public override async Task<ActionResult<PaginationModel<FormViewModel>>> Get()
        {
            bool isStatus = Enum.TryParse(Request.Query[MixRequestQueryKeywords.Status], out MixContentStatus status);
            bool isAttributeId = int.TryParse(Request.Query[MixRequestQueryKeywords.DatabaseId], out int attributeSetId);
            bool isFromDate = DateTime.TryParse(Request.Query[MixRequestQueryKeywords.FromDate], out DateTime fromDate);
            bool isToDate = DateTime.TryParse(Request.Query[MixRequestQueryKeywords.ToDate], out DateTime toDate);
            bool isParentType = Enum.TryParse(Request.Query[MixRequestQueryKeywords.ParentType], out MixDatabaseParentType parentType);
            string parentId = Request.Query[MixRequestQueryKeywords.ParentId];
            string attributeSetName = Request.Query[MixRequestQueryKeywords.DatabaseName];
            Expression<Func<MixRelatedAttributeData, bool>> predicate = model =>
                model.Specificulture == _lang
                && (!isStatus || model.Status == status)
                && (!isFromDate || model.CreatedDateTime >= fromDate)
                && (!isToDate || model.CreatedDateTime <= toDate)
                && ((isAttributeId && model.AttributeSetId == attributeSetId) || model.AttributeSetName == attributeSetName)
                && (string.IsNullOrEmpty(parentId)
                 || (model.ParentId == parentId && model.ParentType == parentType)
                 );

            var getData = await base.GetListAsync<FormViewModel>(predicate);
            if (getData.IsSucceed)
            {
                return Ok(getData.Data);
            }
            else
            {
                return BadRequest(getData.Errors);
            }
        }
    }
}