// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Mvc;
using Mix.Cms.Lib;
using Mix.Cms.Lib.Controllers;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.ViewModels.MixAttributeSetDatas;
using Mix.Domain.Core.ViewModels;
using System.Threading.Tasks;

namespace Mix.Cms.Api.RestFul.Controllers.v1
{
    [Route("api/v1/rest/{culture}/attribute-set-data/portal")]
    public class AttributeSetDataPortalController :
        BaseRestApiController<MixCmsContext, MixAttributeSetData, FormViewModel>
    {
        // GET: api/v1/rest/{culture}/attribute-set-data
        [HttpGet]
        public override async Task<ActionResult<PaginationModel<FormViewModel>>> Get()
        {
            var getData = await Helper.FilterByKeywordAsync<FormViewModel>(_lang, Request);
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
        [HttpGet("init/{attributeSet}")]
        public async Task<ActionResult<FormViewModel>> Init(string attributeSet)
        {
            int.TryParse(attributeSet, out int attributeSetId);
            var getAttrSet = await Lib.ViewModels.MixAttributeSets.UpdateViewModel.Repository.GetSingleModelAsync(m => m.Name == attributeSet || m.Id == attributeSetId);
            if (getAttrSet.IsSucceed)
            {
                FormViewModel result = new FormViewModel() { 
                    Specificulture = _lang,
                    AttributeSetId = getAttrSet.Data.Id,
                    AttributeSetName = getAttrSet.Data.Name,
                    Status = MixEnums.MixContentStatus.Published,
                    Fields = getAttrSet.Data.Fields
                };
                result.ExpandView();
                return Ok(result);
            }            
            else
            {
                return BadRequest(getAttrSet.Errors);
            }
        }

        // DELETE: api/v1/rest/en-us/attribute-set/portal/5
        [HttpDelete("{id}")]
        public override async Task<ActionResult<MixAttributeSetData>> Delete(string id)
        {
            var result = await DeleteAsync<DeleteViewModel>(id, true);
            if (result.IsSucceed)
            {
                return Ok(result.Data);
            }
            else
            {
                return BadRequest(result.Errors);
            }

        }
    }

}