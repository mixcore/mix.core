// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Mvc;
using Mix.Cms.Lib;
using Mix.Cms.Lib.Controllers;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Cms.Lib.ViewModels.MixAttributeSetDatas;
using Mix.Domain.Core.ViewModels;
using System;
using System.Threading.Tasks;

namespace Mix.Cms.Api.RestFul.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1/rest/{culture}/attribute-set-data/client")]
    public class ApiAttributeSetDataController :
        BaseRestApiController<MixCmsContext, MixAttributeSetData, FormViewModel, FormViewModel, FormViewModel>
    {
        // GET: api/v1/rest/{culture}/attribute-set-data/client/search
        [HttpGet]
        public override async Task<ActionResult<PaginationModel<FormViewModel>>> Get()
        {
            var getData = await Helper.FilterByKeywordAsync<FormViewModel>(Request, _lang);
            if (getData.IsSucceed)
            {
                return Ok(getData.Data);
            }
            else
            {
                return BadRequest(getData.Errors);
            }
        }

        // GET: api/v1/rest/{culture}/attribute-set-data
        [HttpGet("init/{attributeSetName}")]
        public async Task<ActionResult<UpdateViewModel>> Init(string attributeSetName)
        {

            var getAttrSet = await Lib.ViewModels.MixAttributeSets.ReadViewModel.Repository.GetSingleModelAsync(m => m.Name == attributeSetName);
            if (getAttrSet.IsSucceed)
            {
                FormViewModel result = new FormViewModel()
                {
                    Specificulture = _lang,
                    AttributeSetId = getAttrSet.Data.Id,
                    AttributeSetName = getAttrSet.Data.Name,
                    Status = MixService.GetEnumConfig<MixEnums.MixContentStatus>(MixConstants.ConfigurationKeyword.DefaultContentStatus),
                };
                result.ExpandView();
                return Ok(result);
            }
            else
            {
                return BadRequest(getAttrSet.Errors);
            }
        }
    }

}