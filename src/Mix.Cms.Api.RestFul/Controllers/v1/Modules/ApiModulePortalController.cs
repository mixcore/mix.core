// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Controllers;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Repositories;
using Mix.Cms.Lib.ViewModels.MixModules;
using Mix.Heart.Infrastructure.Repositories;
using Mix.Heart.Models;
using Mix.Identity.Constants;
using Mix.Identity.Helpers;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mix.Cms.Api.RestFul.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1/rest/{culture}/module/portal")]
    public class ApiModuleController :
        BaseAuthorizedRestApiController<MixCmsContext, MixModule, UpdateViewModel, ReadListItemViewModel, UpdateViewModel>
    {
        public ApiModuleController(
            DefaultRepository<MixCmsContext, MixModule, ReadListItemViewModel> repo, 
            DefaultRepository<MixCmsContext, MixModule, UpdateViewModel> updRepo, 
            DefaultRepository<MixCmsContext, MixModule, UpdateViewModel> delRepo,
            MixIdentityHelper mixIdentityHelper,
            AuditLogRepository auditlogRepo)
            : base(repo, updRepo, delRepo, mixIdentityHelper, auditlogRepo)
        {
        }

        [HttpGet]
        public override async Task<ActionResult<PaginationModel<ReadListItemViewModel>>> Get()
        {
            bool isStatus = Enum.TryParse(Request.Query[MixRequestQueryKeywords.Status], out MixContentStatus status);
            bool isFromDate = DateTime.TryParse(Request.Query[MixRequestQueryKeywords.FromDate], out DateTime fromDate);
            bool isToDate = DateTime.TryParse(Request.Query[MixRequestQueryKeywords.ToDate], out DateTime toDate);
            string keyword = Request.Query[MixRequestQueryKeywords.Keyword];
            Expression<Func<MixModule, bool>> predicate = model =>
                model.Specificulture == _lang
                && (!isStatus || model.Status == status)
                && (!isFromDate || model.CreatedDateTime >= fromDate)
                && (!isToDate || model.CreatedDateTime <= toDate)
                && (string.IsNullOrEmpty(keyword)
                 || (EF.Functions.Like(model.Title, $"%{keyword}%"))
                 || (EF.Functions.Like(model.Description, $"%{keyword}%"))
                 || (EF.Functions.Like(model.Name, $"%{keyword}%"))
                 );
            var getData = await base.GetListAsync<ReadListItemViewModel>(predicate);
            if (getData.IsSucceed)
            {
                return getData.Data;
            }
            else
            {
                return BadRequest(getData.Errors);
            }
        }

        protected override async Task<RepositoryResponse<UpdateViewModel>> SaveAsync(UpdateViewModel vm, bool isSaveSubModel)
        {
            var result = await base.SaveAsync(vm, isSaveSubModel);
            if (result.IsSucceed && vm.IsClone)
            {
                var cloneResult = await vm.CloneAsync(result.Data.Model, vm.Cultures.Where(m=>m.Specificulture != _lang).ToList());
                if (!cloneResult.IsSucceed)
                {
                    result.IsSucceed = false;
                    result.Errors.Add("Cannot clone");
                    result.Errors.AddRange(cloneResult.Errors);
                }
            }
            return result;
        }

        public override async Task<ActionResult<UpdateViewModel>> Duplicate(string id)
        {
            var getData = await GetSingleAsync(id);
            if (getData.IsSucceed)
            {
                var data = getData.Data;
                data.Id = 0;
                data.CreatedDateTime = DateTime.UtcNow;
                data.CreatedBy = _mixIdentityHelper.GetClaim(User, MixClaims.Username);
                data.Name = $"Copy_{data.Name}";
                var result = await data.SaveModelAsync(true);

                if (result.IsSucceed)
                {
                    var getAdditionaData = await Lib.ViewModels.MixDatabaseDataAssociations.UpdateViewModel.Repository.GetFirstModelAsync(
                            m => m.MixDatabaseName == MixDatabaseNames.ADDITIONAL_COLUMN_MODULE
                                && m.ParentType == MixDatabaseParentType.Module
                                && m.ParentId == id
                                && m.Specificulture == _lang);
                    if (getAdditionaData.IsSucceed)
                    {
                        getAdditionaData.Data.ParentId = result.Data.Id.ToString();
                        await getAdditionaData.Data.DuplicateAsync();
                    }
                }
                return GetResponse(result);
            }
            return NotFound();
        }
    }
}