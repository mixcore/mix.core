// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mix.Cms.Lib;
using Mix.Cms.Lib.Controllers;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.ViewModels.MixAttributeSetDatas;
using Mix.Domain.Core.ViewModels;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mix.Cms.Api.RestFul.Controllers.v1
{
    [Route("api/v1/rest/{culture}/attribute-set-data/portal")]
    public class AttributeSetDataPortalController :
        BaseRestApiController<MixCmsContext, MixAttributeSetData, FormPortalViewModel, FormPortalViewModel, FormPortalViewModel>
    {
        // GET: api/v1/rest/{culture}/attribute-set-data
        [HttpGet]
        public override async Task<ActionResult<PaginationModel<FormPortalViewModel>>> Get()
        {
            var getData = await Helper.FilterByKeywordAsync<FormPortalViewModel>(Request, _lang);
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
        public async Task<ActionResult<FormPortalViewModel>> Init(string attributeSet)
        {
            int.TryParse(attributeSet, out int attributeSetId);
            var getAttrSet = await Lib.ViewModels.MixAttributeSets.UpdateViewModel.Repository.GetSingleModelAsync(m => m.Name == attributeSet || m.Id == attributeSetId);
            if (getAttrSet.IsSucceed)
            {
                FormPortalViewModel result = new FormPortalViewModel()
                {
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

        // GET api/attribute-set-data
        [HttpGet("export")]
        public async Task<ActionResult> Export()
        {
            string attributeSetName = Request.Query["attributeSetName"].ToString();
            string exportPath = $"exports/module/{attributeSetName}";
            var getData = await Helper.FilterByKeywordAsync<FormPortalViewModel>(Request, _lang);

            var jData = new List<JObject>();
            if (getData.IsSucceed)
            {
                foreach (var item in getData.Data.Items)
                {
                    jData.Add(item.Obj);
                }
                var result = Lib.ViewModels.MixAttributeSetDatas.Helper.ExportAttributeToExcel(jData, string.Empty, exportPath, $"{attributeSetName}", null);
                return Ok(result.Data);
            }
            else
            {
                return BadRequest(getData.Errors);
            }
        }

        // POST api/attribute-set-data
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin, Admin")]
        [HttpPost, HttpOptions]
        [Route("import-data/{attributeSetName}")]
        public async Task<ActionResult<RepositoryResponse<ImportViewModel>>> ImportData(string attributeSetName, [FromForm] IFormFile file)
        {
            var getAttributeSet = await Lib.ViewModels.MixAttributeSets.ReadViewModel.Repository.GetSingleModelAsync(
                    m => m.Name == attributeSetName);
            if (getAttributeSet.IsSucceed)
            {
                if (file != null)
                {
                    var result = await Helper.ImportData(_lang, getAttributeSet.Data, file);
                    return Ok(result);
                }
            }
            return new RepositoryResponse<ImportViewModel>() { Status = 501 };
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