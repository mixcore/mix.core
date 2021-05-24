// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Mvc;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Controllers;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Cms.Lib.ViewModels.MixDatabaseDatas;
using Mix.Heart.Infrastructure.Repositories;
using Mix.Heart.Models;
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
        [HttpGet("init/{mixDatabaseName}")]
        public async Task<ActionResult<UpdateViewModel>> Init(string mixDatabaseName)
        {
            var getAttrSet = await Lib.ViewModels.MixDatabases.ReadViewModel.Repository.GetSingleModelAsync(m => m.Name == mixDatabaseName);
            if (getAttrSet.IsSucceed)
            {
                FormViewModel result = new FormViewModel()
                {
                    Specificulture = _lang,
                    MixDatabaseId = getAttrSet.Data.Id,
                    MixDatabaseName = getAttrSet.Data.Name,
                    Status = MixService.GetEnumConfig<MixContentStatus>(MixAppSettingKeywords.DefaultContentStatus),
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