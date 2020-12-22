// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Mvc;
using Mix.Cms.Lib;
using Mix.Cms.Lib.Controllers;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.ViewModels.MixTemplates;
using Mix.Domain.Core.ViewModels;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mix.Cms.Api.RestFul.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1/rest/template/portal")]
    public class ApiTemplateController :
        BaseRestApiController<MixCmsContext, MixTemplate, UpdateViewModel, ReadViewModel, DeleteViewModel>
    {

        // GET: api/s
        [HttpGet]
        public override async Task<ActionResult<PaginationModel<ReadViewModel>>> Get()
        {
            bool isStatus = Enum.TryParse(Request.Query["status"], out MixEnums.MixContentStatus status);
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
            var getData = await base.GetListAsync<ReadViewModel>(predicate);
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
        [HttpGet("copy/{id}")]
        public async Task<ActionResult<UpdateViewModel>> Copy(string id)
        {
            var getData = await GetSingleAsync<UpdateViewModel>(id);
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

    }

}