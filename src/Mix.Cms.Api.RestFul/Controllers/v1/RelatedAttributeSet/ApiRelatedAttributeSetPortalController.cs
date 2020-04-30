// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Mvc;
using Mix.Cms.Lib;
using Mix.Cms.Lib.Controllers;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.ViewModels.MixRelatedAttributeSets;
using Mix.Domain.Core.ViewModels;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mix.Cms.Api.RestFul.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1/rest/{culture}/related-attribute-set/portal")]
    public class ApiRelatedAttributeSetPortalController :
        BaseRestApiController<MixCmsContext, MixRelatedAttributeSet, UpdateViewModel>
    {
        // GET: api/v1/rest/{culture}/related-attribute-set
        [HttpGet]
        public override async Task<ActionResult<PaginationModel<UpdateViewModel>>> Get()
        {
            bool isStatus = int.TryParse(Request.Query["status"], out int status);
            bool isFromDate = DateTime.TryParse(Request.Query["fromDate"], out DateTime fromDate);
            bool isToDate = DateTime.TryParse(Request.Query["toDate"], out DateTime toDate);
            string keyword = Request.Query["keyword"];
            string parentType = Request.Query["parentType"];
            string parentId = Request.Query["parentId"];
            Expression<Func<MixRelatedAttributeSet, bool>> predicate = model =>
                (!isStatus || model.Status == status)
                && (!isFromDate || model.CreatedDateTime >= fromDate)
                && (!isToDate || model.CreatedDateTime <= toDate)
                && (string.IsNullOrEmpty(parentId)
                 || model.ParentId.Equals(keyword)
                 )
                && (string.IsNullOrEmpty(parentType)
                 || model.ParentId.Equals(parentType)
                 );
            var getData = await base.GetListAsync(predicate);
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