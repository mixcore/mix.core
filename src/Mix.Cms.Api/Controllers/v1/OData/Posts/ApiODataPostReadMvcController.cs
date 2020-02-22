// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.ViewModels.MixPosts;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mix.Cms.Api.Controllers.v1.OData.Posts
{
    [Produces("application/json")]
    [Route("api/v1/odata/{culture}/post/read-mvc")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin, Admin")]
    public class ApiODataPostReadMvcController :
        BaseApiODataController<MixCmsContext, MixPost>
    {
        public ApiODataPostReadMvcController(
            IMemoryCache memoryCache
            , Microsoft.AspNetCore.SignalR.IHubContext<Hub.PortalHub> hubContext) : base(memoryCache, hubContext)
        {
        }

        #region Get

        // GET api/Posts/id
        [EnableQuery]
        [HttpGet, HttpOptions]
        [Route("{id}")]
        public async Task<ActionResult<ReadMvcViewModel>> Details(string culture, int id)
        {
            string msg = string.Empty;
            Expression<Func<MixPost, bool>> predicate = null;
            MixPost model = null;
            // Get Details if has id or else get default
            if (id > 0)
            {
                predicate = m => m.Id == id;
            }
            else
            {
                model = new MixPost()
                {
                    Priority = ReadMvcViewModel.Repository.Max(p => p.Priority).Data + 1
                };
            }

            var readResult = await base.GetSingleAsync<ReadMvcViewModel>(id.ToString(), predicate, model);

            return Ok(readResult.Data);
        }

        // GET api/Posts/id
        [EnableQuery(MaxExpansionDepth = 4)]
        [HttpGet, HttpOptions]
        [Route("type/{type}")]
        public async Task<ActionResult<JObject>> ListByType(string culture, int type, ODataQueryOptions<MixPost> queryOptions)
        {
            Expression<Func<MixPost, bool>> predicate = m => m.Type == type && m.Specificulture == culture;
            var data = await base.GetListAsync<ReadMvcViewModel>(predicate, $"type_{type}", queryOptions);
            return Ok(data);
        }

        // GET api/Posts/id
        [EnableQuery(MaxExpansionDepth = 4)]
        [HttpGet, HttpOptions]
        public async Task<ActionResult<List<ReadMvcViewModel>>> List(string culture, ODataQueryOptions<MixPost> queryOptions)
        {
            var data = await base.GetListAsync<ReadMvcViewModel>(queryOptions);
            return Ok(data);
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

        // Save api/odata/{culture}/attribute-set/read/{id}
        [HttpPost, HttpOptions]
        [Route("{id}")]
        public async Task<ActionResult<ReadMvcViewModel>> Save(string culture, int id, [FromBody]JObject data)
        {
            var readResult = await base.SaveAsync<ReadMvcViewModel>(data, p => p.Id == id);
            if (readResult.IsSucceed)
            {
                return Ok(readResult);
            }
            else
            {
                return BadRequest(readResult);
            }
        }

        #endregion Get
    }
}