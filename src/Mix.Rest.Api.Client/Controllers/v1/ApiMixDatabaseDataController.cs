// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Mvc;
using Mix.Cms.Lib.Controllers;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.ViewModels.MixDatabaseDatas;
using Mix.Heart.Infrastructure.Repositories;
using Mix.Heart.Models;
using Mix.Rest.Api.Client.Helpers;
using Mix.Rest.Api.Client.ViewModels;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace Mix.Rest.Api.Client.v1
{
    [Produces("application/json")]
    [Route("api/v1/rest/{culture}/mix-database-data/client")]
    public class ApiMixDatabaseDataController :
        BaseReadOnlyApiController<MixCmsContext, MixDatabaseData, DataViewModel>
    {
        public ApiMixDatabaseDataController(
            DefaultRepository<MixCmsContext, MixDatabaseData, DataViewModel> repo) : base(repo)
        {
        }

        // GET: api/v1/rest/{culture}/mix-database-data/client/search
        [HttpGet]
        public override async Task<ActionResult<PaginationModel<DataViewModel>>> Get()
        {
            var getData = await Helper.FilterByKeywordAsync<DataViewModel>(Request, _lang);
            if (getData.IsSucceed)
            {
                return Ok(getData.Data);
            }
            else
            {
                return BadRequest(getData.Errors);
            }
        }
        [HttpGet("search")]
        public ActionResult<PaginationModel<JObject>> Search()
        {
            var getData = MixDatabaseHelper.GetListData<JObject>(Request, _lang);
            return Ok(getData);
        }
    }
}