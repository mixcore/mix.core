// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mix.Cms.Lib;
using Mix.Cms.Lib.Controllers;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.ViewModels.MixAttributeSets;
using Mix.Domain.Core.ViewModels;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mix.Cms.Api.RestFul.Controllers.v1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Produces("application/json")]
    [Route("api/v1/rest/attribute-set/portal")]
    public class ApiAttributeSetPortalController :
        BaseRestApiController<MixCmsContext, MixAttributeSet, UpdateViewModel, ReadViewModel, UpdateViewModel>
    {

        // GET: api/v1/rest/en-us/attribute-set/portal
        [HttpGet]
        public override async Task<ActionResult<PaginationModel<ReadViewModel>>> Get()
        {
            bool isStatus = Enum.TryParse(Request.Query["status"], out MixEnums.MixContentStatus status);
            bool isType = Enum.TryParse(Request.Query["type"], out MixEnums.MixAttributeSetDataType type);
            bool isFromDate = DateTime.TryParse(Request.Query["fromDate"], out DateTime fromDate);
            bool isToDate = DateTime.TryParse(Request.Query["toDate"], out DateTime toDate);
            string keyword = Request.Query["keyword"];
            Expression<Func<MixAttributeSet, bool>> predicate = model =>
                (!isStatus || model.Status == status)
                && (!isType || model.Type == type.ToString())
                && (!isFromDate || model.CreatedDateTime >= fromDate)
                && (!isToDate || model.CreatedDateTime <= toDate)
                && (string.IsNullOrEmpty(keyword)
                 || model.Name.Contains(keyword)
                 || model.Title.Contains(keyword)
                 );
            var getData = await base.GetListAsync<ReadViewModel>(predicate);
            if (getData.IsSucceed)
            {
                return Ok(getData.Data);
            }
            else
            {
                return BadRequest(getData.Errors);
            }
        }

        // DELETE: api/v1/rest/en-us/attribute-set/portal/5
        [HttpDelete("{id}")]
        public override async Task<ActionResult<MixAttributeSet>> Delete(string id)
        {
            var getData = await DeleteViewModel.Repository.GetSingleModelAsync(m => m.Id == int.Parse(id));

            if (getData.IsSucceed)
            {
                var result = await getData.Data.RemoveModelAsync(true);
                if (result.IsSucceed)
                {
                    return Ok(result.Data);
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            else
            {
                return NotFound();
            }

        }
    }

}