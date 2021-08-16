// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Controllers;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Models.Common;
using Mix.Cms.Lib.ViewModels.MixPages;
using Mix.Heart.Infrastructure.Repositories;
using Mix.Heart.Models;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Mix.Heart.Extensions;

namespace Mix.Cms.Api.RestFul.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1/rest/{culture}/mix-page/mvc")]
    public class ApiPageMvcController :
        BaseReadOnlyApiController<MixCmsContext, MixPage, ReadMvcViewModel>
    {
        public ApiPageMvcController(DefaultRepository<MixCmsContext, MixPage, ReadMvcViewModel> repo) 
            : base(repo)
        {
        }

        [HttpGet]
        public override async Task<ActionResult<PaginationModel<ReadMvcViewModel>>> Get()
        {
            bool isStatus = Enum.TryParse(Request.Query[MixRequestQueryKeywords.Status], out MixContentStatus status);
            bool isFromDate = DateTime.TryParse(Request.Query[MixRequestQueryKeywords.FromDate], out DateTime fromDate);
            bool isToDate = DateTime.TryParse(Request.Query[MixRequestQueryKeywords.ToDate], out DateTime toDate);
            string type = Request.Query["type"];
            string keyword = Request.Query[MixRequestQueryKeywords.Keyword];
            Expression<Func<MixPage, bool>> predicate = model => model.Specificulture == _lang;
            predicate = predicate.AndAlsoIf(isStatus, model => model.Status == status);
            predicate = predicate.AndAlsoIf(isFromDate, model => model.CreatedDateTime >= fromDate);
            predicate = predicate.AndAlsoIf(isToDate, model => model.CreatedDateTime <= toDate);
            predicate = predicate.AndAlsoIf(!string.IsNullOrEmpty(keyword), model => (EF.Functions.Like(model.Title, $"%{keyword}%"))
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

        protected override async Task<RepositoryResponse<ReadMvcViewModel>> GetSingleAsync(string id)
        {
            int.TryParse(Request.Query[MixRequestQueryKeywords.PageSize], out int pageSize);
            int.TryParse(Request.Query[MixRequestQueryKeywords.PageIndex], out int pageIndex);
            var result = await base.GetSingleAsync(id);
            result.Data.LoadData(pageSize, pageIndex);
            return result;
        }
    }
}