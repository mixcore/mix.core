// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Mvc;
using Mix.Cms.Lib.Controllers;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.ViewModels.MixDatabaseDatas;
using Mix.Heart.Infrastructure.Repositories;
using Mix.Heart.Models;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace Mix.Cms.Api.RestFul.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1/rest/{culture}/mix-database-data/form")]
    [Route("api/v1/rest/mix-database-data/form")]
    public class ApiMixDatabaseDataController :
        BaseLocalizeRestApiController<MixCmsContext, MixDatabaseData, FormViewModel>
    {
        public ApiMixDatabaseDataController(DefaultRepository<MixCmsContext, MixDatabaseData, FormViewModel> repo) 
            : base(repo)
        {
        }

        // GET: api/v1/rest/{culture}/mix-database-data/client/search
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

        // GET: api/v1/rest/{culture}/mix-database-data
        [HttpGet("init/{mixDatabase}")]
        public async Task<ActionResult<FormViewModel>> Init(string mixDatabase)
        {
            var formData = await Helper.GetFormDataAsync(mixDatabase, _lang);
            return formData != null
                ? Ok(formData)
                : BadRequest(mixDatabase);
        }

        [HttpPost("save-data/{mixDatabase}")]
        public async Task<ActionResult<FormViewModel>> SaveData([FromRoute] string mixDatabase, bool? sendMail, bool? sendSms, [FromBody] JObject data)
        {
            var formData = await Helper.GetFormDataAsync(mixDatabase, _lang);
            if (formData != null)
            {
                formData.Obj = data;
                var result = await SaveAsync(formData, true);
                if (result.IsSucceed)
                {
                    await Helper.SendMail(mixDatabase, _lang, data);
                }
                return GetResponse(result);
            }
            return BadRequest(mixDatabase);
        }
        
        [HttpPost("save-values/{dataId}")]
        public async Task<ActionResult<bool>> SaveValue([FromRoute] string dataId, [FromBody] JObject values)
        {
            var getFormData = await FormViewModel.Repository.GetSingleModelAsync(m => m.Id == dataId && m.Specificulture == _lang);
            if (getFormData.IsSucceed)
            {
                var formData = getFormData.Data;
                formData.UpdateValues(values);
                var result = await formData.SaveValues(formData.ParseModel());
                return GetResponse(result);
            }
            return NotFound(dataId);
        }
    }
}