// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Mvc;
using Mix.Cms.Lib;
using Mix.Cms.Lib.Controllers;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.ViewModels.MixAttributeFields;
using Mix.Domain.Core.ViewModels;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mix.Cms.Api.RestFul.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1/rest/attribute-field/portal")]
    public class ApiAttributeFieldController :
        BaseRestApiController<MixCmsContext, MixAttributeField, UpdateViewModel, ReadViewModel, DeleteViewModel>
    {

        // GET: api/v1/rest/en-us/attribute-field/client
        [HttpGet]
        public override async Task<ActionResult<PaginationModel<ReadViewModel>>> Get()
        {
            bool isStatus = Enum.TryParse(Request.Query["status"], out MixEnums.MixContentStatus status);
            bool isFromDate = DateTime.TryParse(Request.Query["fromDate"], out DateTime fromDate);
            bool isToDate = DateTime.TryParse(Request.Query["toDate"], out DateTime toDate);
            string keyword = Request.Query["keyword"];
            Expression<Func<MixAttributeField, bool>> predicate = model =>
                (!isStatus || model.Status == status)
                && (!isFromDate || model.CreatedDateTime >= fromDate)
                && (!isToDate || model.CreatedDateTime <= toDate)
                && (string.IsNullOrEmpty(keyword)
                 || model.AttributeSetName.Contains(keyword)
                 || model.Name.Contains(keyword)
                 || model.DefaultValue.Contains(keyword)
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
        // GET: api/v1/rest/en-us/attribute-field/client
        [HttpGet("init/{attributeSet}")]
        public async Task<ActionResult<PaginationModel<UpdateViewModel>>> Init(string attributeSet)
        {
            int.TryParse(attributeSet, out int attributeSetId);
            var getData = await UpdateViewModel.Repository.GetModelListByAsync(f => f.AttributeSetName == attributeSet || f.AttributeSetId == attributeSetId
            , _context, _transaction);

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