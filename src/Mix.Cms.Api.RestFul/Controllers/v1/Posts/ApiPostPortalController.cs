// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Mvc;
using Mix.Cms.Lib.Attributes;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Controllers;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Models.Common;
using Mix.Cms.Lib.Repositories;
using Mix.Cms.Lib.Services;
using Mix.Cms.Lib.ViewModels.MixPosts;
using Mix.Heart.Infrastructure.Repositories;
using Mix.Heart.Models;
using Mix.Identity.Helpers;
using System.Linq;
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
            MixIdentityHelper mixIdentityHelper,
            AuditLogRepository auditlogRepo) 
            : base(repo, updRepo, delRepo, mixIdentityHelper, auditlogRepo)
        {
        }

        [MixAuthorize]
        [HttpGet]
        public override async Task<ActionResult<PaginationModel<ReadViewModel>>> Get()
        {
            var searchPostData = new SearchPostQueryModel(Request, _lang);
            var getData = await Helper.SearchPosts<ReadViewModel>(searchPostData);
            return GetResponse(getData);
        }

        protected override async Task<RepositoryResponse<UpdateViewModel>> SaveAsync(UpdateViewModel vm, bool isSaveSubModel)
        {
            var result = await base.SaveAsync(vm, isSaveSubModel);
            if (result.IsSucceed && vm.IsClone)
            {
                var cloneResult = await vm.CloneAsync(result.Data.Model, vm.Cultures.Where(m => m.Specificulture != _lang).ToList());
                if (!cloneResult.IsSucceed)
                {
                    result.IsSucceed = false;
                    result.Errors.Add("Cannot clone");
                    result.Errors.AddRange(cloneResult.Errors);
                }
            }
            return result;
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