// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Controllers;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Cms.Lib.ViewModels.MixPosts;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.Repository;
using Mix.Identity.Helpers;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mix.Cms.Api.RestFul.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1/rest/{culture}/post/portal")]
    public class ApiPostController :
        BaseAuthorizedRestApiController<MixCmsContext, MixPost, UpdateViewModel, ReadViewModel, DeleteViewModel>
    {
        public ApiPostController(
            DefaultRepository<MixCmsContext, MixPost, ReadViewModel> repo, 
            DefaultRepository<MixCmsContext, MixPost, UpdateViewModel> updRepo, 
            DefaultRepository<MixCmsContext, MixPost, DeleteViewModel> delRepo,
            MixIdentityHelper mixIdentityHelper) : base(repo, updRepo, delRepo, mixIdentityHelper)
        {
        }
        
        [HttpGet]
        public override async Task<ActionResult<PaginationModel<ReadViewModel>>> Get()
        {
            bool isStatus = Enum.TryParse(Request.Query[MixRequestQueryKeywords.Status], out MixContentStatus status);
            bool isFromDate = DateTime.TryParse(Request.Query[MixRequestQueryKeywords.FromDate], out DateTime fromDate);
            bool isToDate = DateTime.TryParse(Request.Query[MixRequestQueryKeywords.ToDate], out DateTime toDate);
            string type = Request.Query["type"];
            string keyword = Request.Query[MixRequestQueryKeywords.Keyword];
            Expression<Func<MixPost, bool>> predicate = model =>
                model.Specificulture == _lang
                && (!isStatus || model.Status == status)
                && (!isFromDate || model.CreatedDateTime >= fromDate)
                && (!isToDate || model.CreatedDateTime <= toDate)
                && (string.IsNullOrEmpty(type) || model.Type == type)
                && (string.IsNullOrEmpty(keyword)
                 || (EF.Functions.Like(model.Title, $"%{keyword}%"))
                 || (EF.Functions.Like(model.Excerpt, $"%{keyword}%"))
                 || (EF.Functions.Like(model.Content, $"%{keyword}%"))
                 );
            var getData = await base.GetListAsync<ReadViewModel>(predicate);
            if (getData.IsSucceed)
            {
                return getData.Data;
            }
            else
            {
                return BadRequest(getData.Errors);
            }
        }

        public override ActionResult<UpdateViewModel> Default()
        {
            using (MixCmsContext context = new MixCmsContext())
            {
                var transaction = context.Database.BeginTransaction();
                string template = !string.IsNullOrEmpty(Request.Query["template"].ToString())
                        ? $"{Request.Query["template"]}.cshtml"
                        : null;
                var model = new MixPost()
                {
                    Specificulture = _lang,
                    Status = MixService.GetEnumConfig<MixContentStatus>(MixAppSettingKeywords.DefaultContentStatus),
                    Type = Request.Query["type"].ToString(),
                    Template = template
                };
                var result = new UpdateViewModel(model, context, transaction);
                return Ok(result);
            }
        }
    }
}