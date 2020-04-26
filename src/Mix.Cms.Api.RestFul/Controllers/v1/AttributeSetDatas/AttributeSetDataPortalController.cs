// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Mvc;
using Mix.Cms.Lib;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.ViewModels.MixAttributeSetDatas;
using Mix.Domain.Core.ViewModels;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mix.Cms.Api.RestFul.Controllers.v1
{
    [Route("api/v1/rest/{culture}/attribute-set-data/portal")]
    public class AttributeSetDataPortalController :
        BaseRestApiController<MixCmsContext, MixAttributeSetData, FormViewModel>
    {
        // GET: api/v1/rest/{culture}/attribute-set-data
        [HttpGet("search")]
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

        // GET: api/v1/rest/{culture}/attribute-set-data/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FormViewModel>> Get(string id)
        {
            Expression<Func<MixAttributeSetData, bool>> predicate = null;
            MixAttributeSetData risk = null;
            if (id == MixConstants.CONST_DEFAULT_STRING_ID)
            {
                var data = new UpdateViewModel();
                data.ExpandView();
                return Ok(data);
            }
            else
            {
                predicate = model => model.Specificulture == _lang && (model.Id == id);
            
            var getData = await base.GetSingleAsync<FormViewModel>(predicate);
                if (getData.IsSucceed)
                {
                    return getData.Data;
                }
                else
                {
                    return NotFound();
                }
            }
        }

        // PUT: api/v1/rest/{culture}/attribute-set-data/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody]FormViewModel data)
        {
            if (id != data.Id)
            {
                return BadRequest();
            }
            var result = await base.SaveAsync(data, true);
            if (result.IsSucceed)
            {
                return NoContent();
            }
            else
            {
                if (!Exists(id))
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
        }

        // POST: api/v1/rest/{culture}/attribute-set-data
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<MixAttributeSetData>> Post([FromBody]JObject obj)
        {
            
            if (obj != null)
            {
                FormViewModel data = obj.ToObject<FormViewModel>();
                data.CreatedBy = User.Identity?.Name;
                var result = await SaveAsync(data, true);
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
                return BadRequest(obj);
            }
        }

        // DELETE: api/v1/rest/{culture}/attribute-set-data/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<MixAttributeSetData>> Delete(string id)
        {
            Expression<Func<MixAttributeSetData, bool>> predicate = m => m.Id == id && m.Specificulture == _lang;
            var result = await base.DeleteAsync<DeleteViewModel>(predicate, true);
            if (result.IsSucceed)
            {
                return Ok(result.Data);
            }
            else
            {
                return BadRequest(result.Errors);
            }

        }

        private bool Exists(string id)
        {
            return UpdateViewModel.Repository.CheckIsExists(e => e.Id == id);
        }
    }

}