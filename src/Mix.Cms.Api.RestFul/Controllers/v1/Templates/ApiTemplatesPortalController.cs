// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Mvc;
using Mix.Cms.Lib;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.ViewModels.MixTemplates;
using Mix.Domain.Core.ViewModels;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mix.Cms.Api.RestFul.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1/rest/{culture}/template/portal")]
    public class ApiTemplateController :
        BaseRestApiController<MixCmsContext, MixTemplate>
    {

        // GET: api/s
        [HttpGet]
        public async Task<ActionResult<PaginationModel<UpdateViewModel>>> Get()
        {
            bool isStatus = int.TryParse(Request.Query["status"], out int status);
            bool isFromDate = DateTime.TryParse(Request.Query["fromDate"], out DateTime fromDate);
            bool isToDate = DateTime.TryParse(Request.Query["toDate"], out DateTime toDate);
            string keyword = Request.Query["keyword"];
            bool isTheme = int.TryParse(Request.Query["themeId"], out int themeId);
            string folderType = Request.Query["folderType"];
            Expression<Func<MixTemplate, bool>> predicate = model =>
                (!isStatus || model.Status == status)
                && (!isTheme || model.ThemeId == themeId)
                && (!isFromDate || model.CreatedDateTime >= fromDate)
                && (!isToDate || model.CreatedDateTime <= toDate)
                && (string.IsNullOrEmpty(folderType) || model.FolderType == folderType)
                && (string.IsNullOrEmpty(keyword)
                 || model.FileName.Contains(keyword)
                 || model.Content.Contains(keyword)
                 );
            var getData = await base.GetListAsync<UpdateViewModel>(predicate);
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
        public async Task<ActionResult<UpdateViewModel>> Get(int id)
        {
            Expression<Func<MixTemplate, bool>> predicate = null;
            MixTemplate risk = null;
            if (id == 0)
            {
                risk = new MixTemplate()
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
        
        // GET: api/s/5
        [HttpGet("copy/{id}")]
        public async Task<ActionResult<UpdateViewModel>> Copy(int id)
        {
            Expression<Func<MixTemplate, bool>> predicate = null;
            MixTemplate risk = null;
            if (id == 0)
            {
                return NotFound(id);
            }
            else
            {
                predicate = model => (model.Id == id);
            }
            var getData = await base.GetSingleAsync<UpdateViewModel>(predicate, risk);
            if (getData.IsSucceed)
            {
                var copyResult = await getData.Data.CopyAsync();
                if (copyResult.IsSucceed)
                {
                    return Ok(copyResult.Data);

                }
                else
                {
                    return BadRequest(copyResult.Errors);
                }
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
        public async Task<IActionResult> Put(int id, [FromBody]UpdateViewModel MixTemplate)
        {
            if (id != MixTemplate.Id)
            {
                return BadRequest();
            }
            var result = await base.SaveAsync(MixTemplate, true);
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
        // PATCH: api/v1/rest/en-us/attribute-set/portal/5
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, [FromBody]JObject fields)
        {
            var result = await base.GetSingleAsync<UpdateViewModel>(m => m.Id == id, null);
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
        // POST: api/s
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<MixTemplate>> Post([FromBody]UpdateViewModel MixTemplate)
        {
            var result = await SaveAsync(MixTemplate, true);
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
        public async Task<ActionResult<MixTemplate>> Delete(int id)
        {
            Expression<Func<MixTemplate, bool>> predicate = m => m.Id == id;
            var result = await base.DeleteAsync<DeleteViewModel>(predicate, false);
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