// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Mvc;
using Mix.Cms.Lib.Controllers;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.ViewModels.MixDatabaseDatas;
using Mix.Heart.Infrastructure.Repositories;
using Mix.Heart.Models;
using System.Threading.Tasks;

namespace Mix.Cms.Api.RestFul.Controllers.v1
{
    [Route("api/v1/rest/{culture}/mix-database-data/mvc")]
    public class MixDatabaseDataMvcController :
        BaseReadOnlyApiController<MixCmsContext, MixDatabaseData, ReadMvcViewModel>
    {
        public MixDatabaseDataMvcController(DefaultRepository<MixCmsContext, MixDatabaseData, ReadMvcViewModel> repo) 
            : base(repo)
        {
        }

        // GET: api/v1/rest/{culture}/mix-database-data
        [HttpGet]
        public override async Task<ActionResult<PaginationModel<ReadMvcViewModel>>> Get()
        {
            var getData = await Helper.FilterByKeywordAsync<ReadMvcViewModel>(Request, _lang);
            if (getData.IsSucceed)
            {
                return Ok(getData.Data);
            }
            else
            {
                return BadRequest(getData.Errors);
            }
        }
    }
}