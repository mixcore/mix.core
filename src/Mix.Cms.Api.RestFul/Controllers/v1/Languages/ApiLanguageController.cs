// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Mvc;
using Mix.Cms.Lib;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.ViewModels.MixLanguages;
using Mix.Domain.Core.ViewModels;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mix.Cms.Api.RestFul.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1/rest/{culture}/language")]
    public class ApiLanguageController :
        BaseRestApiController<MixCmsContext, MixLanguage>
    {

        // GET: api/s
        [HttpGet]
        public async Task<ActionResult<PaginationModel<ReadMvcViewModel>>> Get()
        {
            bool isStatus = int.TryParse(Request.Query["status"], out int status);
            bool isFromDate = DateTime.TryParse(Request.Query["fromDate"], out DateTime fromDate);
            bool isToDate = DateTime.TryParse(Request.Query["toDate"], out DateTime toDate);
            string keyword = Request.Query["keyword"];
            Expression<Func<MixLanguage, bool>> predicate = model =>
                model.Specificulture == _lang
                && (!isStatus || model.Status == status)
                && (!isFromDate || model.CreatedDateTime >= fromDate)
                && (!isToDate || model.CreatedDateTime <= toDate)
                && (string.IsNullOrEmpty(keyword)
                 || model.Keyword.Contains(keyword)
                 || model.Value.Contains(keyword)
                 || model.DefaultValue.Contains(keyword)
                 );
            var getData = await base.GetListAsync<ReadMvcViewModel>(predicate);
            if (getData.IsSucceed)
            {
                return getData.Data;
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
            Expression<Func<MixLanguage, bool>> predicate = model =>
                model.Specificulture == _lang
                && (!isStatus || model.Status == status)
                && (!isFromDate || model.CreatedDateTime >= fromDate)
                && (!isToDate || model.CreatedDateTime <= toDate)
                && (string.IsNullOrEmpty(keyword)
                 || model.Keyword.Contains(keyword)
                 || model.Value.Contains(keyword)
                 || model.DefaultValue.Contains(keyword)
                 );
            var getData = await ReadMvcViewModel.Repository.CountAsync(predicate);
            if (getData.IsSucceed)
            {
                return getData.Data;
            }
            else
            {
                return BadRequest(getData.Errors);
            }
        }

        // GET: api/s/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UpdateViewModel>> Get(string id)
        {
            Expression<Func<MixLanguage, bool>> predicate = null;
            MixLanguage risk = null;
            if (id == MixConstants.CONST_DEFAULT_STRING_ID)
            {
                risk = new MixLanguage()
                {
                    Specificulture = _lang
                };
            }
            else
            {
                predicate = model => model.Specificulture == _lang && (model.Keyword == id);
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

        // PUT: api/s/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody]UpdateViewModel MixLanguage)
        {
            if (id != MixLanguage.Keyword)
            {
                return BadRequest();
            }
            var result = await base.SaveAsync(MixLanguage, true);
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

        // POST: api/s
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<MixLanguage>> Post([FromBody]UpdateViewModel MixLanguage)
        {
            var result = await SaveAsync(MixLanguage, true);
            if (result.IsSucceed)
            {
                return Ok(result.Data);
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        // DELETE: api/s/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<MixLanguage>> Delete(string id)
        {
            Expression<Func<MixLanguage, bool>> predicate = m => m.Keyword == id && m.Specificulture == _lang;
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

        private bool Exists(string id)
        {
            return UpdateViewModel.Repository.CheckIsExists(e => e.Keyword == id);
        }
    }

}