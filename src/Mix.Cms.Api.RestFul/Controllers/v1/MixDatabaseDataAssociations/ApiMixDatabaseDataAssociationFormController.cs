// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Mvc;
using Mix.Cms.Lib.Controllers;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.ViewModels.MixDatabaseDataAssociations;
using Mix.Heart.Infrastructure.Repositories;
using Mix.Heart.Models;
using Mix.Identity.Helpers;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mix.Cms.Api.RestFul.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1/rest/{culture}/mix-database-data-association/form")]
    public class ApiMixDatabaseDataAssociationFormController :
        BaseLocalizeRestApiController<MixCmsContext, MixDatabaseDataAssociation, FormViewModel>
    {
        public ApiMixDatabaseDataAssociationFormController(
            DefaultRepository<MixCmsContext, MixDatabaseDataAssociation, FormViewModel> repo, 
            MixIdentityHelper mixIdentityHelper) : base(repo)
        {
        }

        // GET: api/v1/rest/{culture}/mix-database-data-association
        [HttpGet]
        public override async Task<ActionResult<PaginationModel<FormViewModel>>> Get()
        {
            bool isStatus = Enum.TryParse(Request.Query["status"], out MixContentStatus status);
            bool isAttributeId = int.TryParse(Request.Query["mixDatabaseId"], out int mixDatabaseId);
            bool isFromDate = DateTime.TryParse(Request.Query["fromDate"], out DateTime fromDate);
            bool isToDate = DateTime.TryParse(Request.Query["toDate"], out DateTime toDate);
            bool isParentType = Enum.TryParse(Request.Query["parentType"], out MixDatabaseParentType parentType);
            string parentId = Request.Query["parentId"];
            string mixDatabaseName = Request.Query["mixDatabaseName"];
            Expression<Func<MixDatabaseDataAssociation, bool>> predicate = model =>
                model.Specificulture == _lang
                && (!isStatus || model.Status == status)
                && (!isFromDate || model.CreatedDateTime >= fromDate)
                && (!isToDate || model.CreatedDateTime <= toDate)
                && ((isAttributeId && model.MixDatabaseId == mixDatabaseId) || model.MixDatabaseName == mixDatabaseName)
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