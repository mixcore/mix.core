// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Mvc;
using Mix.Cms.Lib;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.ViewModels.MixRelatedAttributeDatas;
using Mix.Domain.Core.ViewModels;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mix.Cms.Api.RestFul.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1/rest/{culture}/related-attribute-data/portal")]
    public class ApiRelatedAttributeDataPortalController :
        BaseRestApiController<MixCmsContext, MixRelatedAttributeData>
    {
        // GET: api/v1/rest/{culture}/related-attribute-data
        [HttpGet]
        public async Task<ActionResult<PaginationModel<FormViewModel>>> Get()
        {
            bool isStatus = int.TryParse(Request.Query["status"], out int status);
            bool isFromDate = DateTime.TryParse(Request.Query["fromDate"], out DateTime fromDate);
            bool isToDate = DateTime.TryParse(Request.Query["toDate"], out DateTime toDate);
            int.TryParse(Request.Query["parentType"], out int parentType);
            string parentId = Request.Query["parentId"];
            Expression<Func<MixRelatedAttributeData, bool>> predicate = model =>
                (!isStatus || model.Status == status)
                && (!isFromDate || model.CreatedDateTime >= fromDate)
                && (!isToDate || model.CreatedDateTime <= toDate)
                && (string.IsNullOrEmpty(parentId)
                 || (model.ParentId.Equals(parentId) && model.ParentType == parentType)
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
        
        
        // GET: api/v1/rest/{culture}/related-attribute-data/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FormViewModel>> Get(string id)
        {
            Expression<Func<MixRelatedAttributeData, bool>> predicate = null;
            MixRelatedAttributeData risk = null;
            if (id == MixConstants.CONST_DEFAULT_STRING_ID)
            {
                var data = new FormViewModel();
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

        // PUT: api/v1/rest/{culture}/related-attribute-data/5
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

        // POST: api/v1/rest/{culture}/related-attribute-data
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<MixRelatedAttributeData>> Post([FromBody]FormViewModel data)
        {
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
        // PATCH: api/v1/rest/en-us/related-attribute-data/portal/5
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(string id, [FromBody]JObject fields)
        {
            var result = await base.GetSingleAsync<FormViewModel>(m => m.Id == id);
            if (result.IsSucceed)
            {
                var saveResult = await result.Data.UpdateFieldsAsync(fields);
                if (saveResult.IsSucceed)
                {
                    return NoContent();
                }
                else
                {
                    return BadRequest(saveResult.Errors);
                }
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
        // DELETE: api/v1/rest/{culture}/related-attribute-data/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<MixRelatedAttributeData>> Delete(string id)
        {
            Expression<Func<MixRelatedAttributeData, bool>> predicate = m => m.Id == id && m.Specificulture == _lang;
            var result = await base.DeleteAsync<FormViewModel>(predicate, false);
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
            return FormViewModel.Repository.CheckIsExists(e => e.Id == id);
        }
    }

}