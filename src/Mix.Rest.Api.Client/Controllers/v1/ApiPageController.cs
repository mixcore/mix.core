// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Controllers;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;
using Mix.Heart.Infrastructure.Repositories;
using Mix.Heart.Models;
using Mix.Heart.Extensions;
using Mix.Rest.Api.Client.ViewModels;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mix.Rest.Api.Client.v1
{
    [Produces("application/json")]
    [Route("api/v1/rest/{culture}/page/client")]
    public class ApiPageController :
        BaseReadOnlyApiController<MixCmsContext, MixPage, PageViewModel>
    {
        public ApiPageController(DefaultRepository<MixCmsContext, MixPage, PageViewModel> repo) 
            : base(repo)
        {
        }

        [HttpGet]
        public override async Task<ActionResult<PaginationModel<PageViewModel>>> Get()
        {
            bool isStatus = Enum.TryParse(Request.Query[MixRequestQueryKeywords.Status], out MixContentStatus status);
            bool isFromDate = DateTime.TryParse(Request.Query[MixRequestQueryKeywords.FromDate], out DateTime fromDate);
            bool isToDate = DateTime.TryParse(Request.Query[MixRequestQueryKeywords.ToDate], out DateTime toDate);
            string keyword = Request.Query[MixRequestQueryKeywords.Keyword];

            Expression<Func<MixPage, bool>> predicate = model => model.Specificulture == _lang;
            predicate = predicate.AndAlsoIf(isStatus, model => model.Status == status);
            predicate = predicate.AndAlsoIf(!isStatus, model => model.Status == MixContentStatus.Published);
            predicate = predicate.AndAlsoIf(isFromDate, model => model.CreatedDateTime >= fromDate);
            predicate = predicate.AndAlsoIf(isToDate, model => model.CreatedDateTime >= toDate);
            predicate = predicate.AndAlsoIf(!string.IsNullOrEmpty(keyword), model => 
                (EF.Functions.Like(model.Title, $"%{keyword}%"))
                 || (EF.Functions.Like(model.Excerpt, $"%{keyword}%"))
                 || (EF.Functions.Like(model.Content, $"%{keyword}%")));

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