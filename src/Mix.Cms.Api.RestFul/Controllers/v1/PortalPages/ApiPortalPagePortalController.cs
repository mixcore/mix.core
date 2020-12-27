// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mix.Cms.Lib;
using Mix.Cms.Lib.Controllers;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.ViewModels.MixPortalPages;
using Mix.Domain.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mix.Cms.Api.RestFul.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1/rest/permission")]
    public class ApiPortalPageController :
        BaseRestApiController<MixCmsContext, MixPortalPage, UpdateViewModel, ReadViewModel>
    {

        // GET: api/s
        [HttpGet]
        public override async Task<ActionResult<PaginationModel<ReadViewModel>>> Get()
        {
            bool isStatus = Enum.TryParse(Request.Query["status"], out MixEnums.MixContentStatus status);
            bool isFromDate = DateTime.TryParse(Request.Query["fromDate"], out DateTime fromDate);
            bool isToDate = DateTime.TryParse(Request.Query["toDate"], out DateTime toDate);
            string keyword = Request.Query["keyword"];
            var exceptIds = Request.Query["exceptIds"].ToString();
            var lstExceptIds = !string.IsNullOrEmpty(exceptIds) ? exceptIds.Split(',').Select(m => int.Parse(m)).ToList()
                : new List<int>();

            bool isLevel = int.TryParse(Request.Query["level"], out int level);
            Expression<Func<MixPortalPage, bool>> predicate = model =>
                (!isStatus || model.Status == status)
                && (!isFromDate || model.CreatedDateTime >= fromDate)
                && (!isToDate || model.CreatedDateTime <= toDate)
                && (!isLevel || model.Level == level)
                && (string.IsNullOrEmpty(exceptIds) || !lstExceptIds.Any(m => m == model.Id))
                && (string.IsNullOrEmpty(keyword)
                 || (EF.Functions.Like(model.TextKeyword, $"%{keyword}%"))
                 || (EF.Functions.Like(model.TextDefault, $"%{keyword}%"))
                 || (EF.Functions.Like(model.Url, $"%{keyword}%"))
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