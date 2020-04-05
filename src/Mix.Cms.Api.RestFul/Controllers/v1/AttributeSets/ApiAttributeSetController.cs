// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Mvc;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.ViewModels.MixAttributeSets;
using Mix.Domain.Core.ViewModels;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mix.Cms.Api.RestFul.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1/rest/{culture}/attribute-set/client")]
    public class ApiAttributeSetController :
        BaseRestApiController<MixCmsContext, MixAttributeSet>
    {

        // GET: api/v1/rest/en-us/attribute-set/client
        [HttpGet]
        public async Task<ActionResult<PaginationModel<ReadViewModel>>> Get()
        {
            bool isStatus = int.TryParse(Request.Query["status"], out int status);
            bool isFromDate = DateTime.TryParse(Request.Query["fromDate"], out DateTime fromDate);
            bool isToDate = DateTime.TryParse(Request.Query["toDate"], out DateTime toDate);
            string keyword = Request.Query["keyword"];
            Expression<Func<MixAttributeSet, bool>> predicate = model =>
                (!isStatus || model.Status == status)
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
        
        // GET: 
        [HttpGet("count")]
        public async Task<ActionResult<int>> Count()
        {
            bool isStatus = int.TryParse(Request.Query["status"], out int status);
            bool isFromDate = DateTime.TryParse(Request.Query["fromDate"], out DateTime fromDate);
            bool isToDate = DateTime.TryParse(Request.Query["toDate"], out DateTime toDate);
            string keyword = Request.Query["keyword"];
            Expression<Func<MixAttributeSet, bool>> predicate = model =>
                (!isStatus || model.Status == status)
                && (!isFromDate || model.CreatedDateTime >= fromDate)
                && (!isToDate || model.CreatedDateTime <= toDate)
                && (string.IsNullOrEmpty(keyword)
                 || model.Name.Contains(keyword)
                 || model.Title.Contains(keyword)
                 );
            var getData = await ReadViewModel.Repository.CountAsync(predicate);
            if (getData.IsSucceed)
            {
                return getData.Data;
            }
            else
            {
                return BadRequest(getData.Errors);
            }
        }

        // GET: api/v1/rest/en-us/attribute-set/client/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UpdateViewModel>> Get(int id)
        {
            Expression<Func<MixAttributeSet, bool>> predicate = null;
            MixAttributeSet risk = null;
            if (id == 0)
            {
                risk = new MixAttributeSet()
                {
                };
            }
            else
            {
                predicate = model => (model.Id == id);
            }
            var getData = await base.GetSingleAsync<UpdateViewModel>(predicate, risk);
            if (getData.IsSucceed)
            {
                return getData.Data;
            }
            else
            {
                return NotFound();
            }
        }

        // PUT: api/v1/rest/en-us/attribute-set/client/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]UpdateViewModel MixAttributeSet)
        {
            if (id != MixAttributeSet.Id)
            {
                return BadRequest();
            }
            var result = await base.SaveAsync(MixAttributeSet, true);
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

        // POST: api/v1/rest/en-us/attribute-set/client
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<MixAttributeSet>> Post([FromBody]UpdateViewModel MixAttributeSet)
        {
            var result = await SaveAsync(MixAttributeSet, true);
            if (result.IsSucceed)
            {
                return Ok(result.Data);
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        // DELETE: api/v1/rest/en-us/attribute-set/client/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<MixAttributeSet>> Delete(int id)
        {
            Expression<Func<MixAttributeSet, bool>> predicate = m => m.Id == id;
            var result = await base.DeleteAsync<UpdateViewModel>(predicate, false);
            if (result.IsSucceed)
            {
                return Ok(result.Data);
            }
            else
            {
                return BadRequest(result.Errors);
            }

        }

        private bool Exists(int id)
        {
            return UpdateViewModel.Repository.CheckIsExists(e => e.Id == id);
        }
    }

}