// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.ViewModels.MixConfigurations;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mix.Cms.Api.Controllers.v1.OData.Configurations
{
    [Produces("application/json")]
    [Route("api/v1/odata/configuration/read-mvc")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin, Admin")]
    public class ApiODataConfigurationReadController :
        BaseApiODataController<MixCmsContext, MixConfiguration>
    {
        public ApiODataConfigurationReadController(
            IMemoryCache memoryCache
            , Microsoft.AspNetCore.SignalR.IHubContext<Hub.PortalHub> hubContext) : base(memoryCache, hubContext)
        {
        }

        #region Get

        // GET api/Configurations/keyword
        [EnableQuery]
        [HttpGet, HttpOptions]
        [Route("{keyword}")]
        public async Task<ActionResult<ReadMvcViewModel>> Details(string culture, string keyword)
        {
            string msg = string.Empty;
            Expression<Func<MixConfiguration, bool>> predicate = null;
            MixConfiguration model = null;
            // Get Details if has keyword or else get default
            if (keyword!="default")
            {
                predicate = m => m.Keyword == keyword;
            }
            else
            {
                model = new MixConfiguration()
                {
                    Priority = ReadMvcViewModel.Repository.Max(p => p.Priority).Data + 1
                };
            }

            var readResult = await base.GetSingleAsync<ReadMvcViewModel>(keyword, predicate, model);

            return Ok(readResult.Data);
        }
        // GET api/Configurations/keyword
        [EnableQuery(MaxExpansionDepth = 4)]
        [HttpGet, HttpOptions]
        [Route("type/{type}")]
        public async Task<ActionResult<List<ReadMvcViewModel>>> ListByType(string culture, string type, ODataQueryOptions<MixConfiguration> queryOptions)
        {
            Expression<Func<MixConfiguration, bool>> predicate = m => m.Category == type && m.Specificulture== culture;
            var result = await base.GetListAsync<ReadMvcViewModel>(predicate, $"type_{type}", queryOptions);
            return Ok(result);
        }

        // GET api/Configurations/keyword
        [EnableQuery(MaxExpansionDepth = 4)]
        [HttpGet, HttpOptions]
        public async Task<ActionResult<List<ReadMvcViewModel>>> List(string culture, ODataQueryOptions<MixConfiguration> queryOptions)
        {
            var result = await base.GetListAsync<ReadMvcViewModel>(queryOptions);
            return Ok(result);
        }
        // GET api/attribute-sets/read/count
        [AllowAnonymous]
        [EnableQuery]
        [Route("count")]
        [HttpGet, HttpOptions]
        public async System.Threading.Tasks.Task<ActionResult<int>> CountAsync()
        {
            return (await ReadMvcViewModel.Repository.CountAsync()).Data;
        }

        // Save api/odata/{culture}/attribute-set/read
        [HttpPost, HttpOptions]
        [Route("")]
        public async Task<ActionResult<ReadMvcViewModel>> Save(string culture, [FromBody]ReadMvcViewModel data)
        {
            var readResult = await base.SaveAsync<ReadMvcViewModel>(data, true);
            if (readResult.IsSucceed)
            {
                return Ok(readResult);
            }
            else
            {
                return BadRequest(readResult);
            }
        }

        // Save api/odata/{culture}/attribute-set/read/{keyword}
        [HttpPost, HttpOptions]
        [Route("{keyword}")]
        public async Task<ActionResult<ReadMvcViewModel>> Save(string culture, string keyword, [FromBody]JObject data)
        {
            var readResult = await base.SaveAsync<ReadMvcViewModel>(data, p => p.Keyword == keyword);
            if (readResult.IsSucceed)
            {
                return Ok(readResult);
            }
            else
            {
                return BadRequest(readResult);
            }
        }

        [HttpDelete, HttpOptions]
        [Route("{keyword}")]
        public async Task<ActionResult<DeleteViewModel>> Delete(string culture, string keyword)
        {
            Expression<Func<MixConfiguration, bool>> predicate = model => model.Keyword == keyword;

            // Get Details if has keyword or else get default

            var readResult = await base.GetSingleAsync<DeleteViewModel>(keyword.ToString(), predicate);

            var result = await base.DeleteAsync<DeleteViewModel>(readResult.Data, true);
            if (result.IsSucceed)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        #endregion Get

    }
}