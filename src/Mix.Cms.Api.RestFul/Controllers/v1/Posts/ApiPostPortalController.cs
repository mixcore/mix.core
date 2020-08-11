// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mix.Cms.Lib;
using Mix.Cms.Lib.Controllers;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.ViewModels.MixPosts;
using Mix.Domain.Core.ViewModels;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mix.Cms.Api.RestFul.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1/rest/{culture}/post/portal")]
    public class ApiPostController :
        BaseRestApiController<MixCmsContext, MixPost, UpdateViewModel, ReadMvcViewModel>
    {

        // GET: api/s
        [HttpGet]
        public override async Task<ActionResult<PaginationModel<ReadMvcViewModel>>> Get()
        {
            bool isStatus = Enum.TryParse(Request.Query["status"], out MixEnums.MixContentStatus status);
            bool isFromDate = DateTime.TryParse(Request.Query["fromDate"], out DateTime fromDate);
            bool isToDate = DateTime.TryParse(Request.Query["toDate"], out DateTime toDate);
            string keyword = Request.Query["keyword"];
            Expression<Func<MixPost, bool>> predicate = model =>
                model.Specificulture == _lang
                && (!isStatus || model.Status == status.ToString())
                && (!isFromDate || model.CreatedDateTime >= fromDate)
                && (!isToDate || model.CreatedDateTime <= toDate)
                && (string.IsNullOrEmpty(keyword)
                 || (EF.Functions.Like(model.Title, $"%{keyword}%"))
                 || (EF.Functions.Like(model.Excerpt, $"%{keyword}%"))
                 || (EF.Functions.Like(model.Content, $"%{keyword}%"))
                 );
            var getData = await base.GetListAsync(predicate);
            if (getData.IsSucceed)
            {
                return getData.Data;
            }
            else
            {
                return BadRequest(getData.Errors);
            }
        }
    }

}