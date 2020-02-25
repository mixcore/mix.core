// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.ViewModels.MixConfigurations;
using Newtonsoft.Json.Linq;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mix.Cms.Api.Controllers.v1.OData.Configurations
{
    [Produces("application/json")]
    [Route("api/v1/odata/{culture}/configuration/mobile")]
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [EnableQuery]
        [HttpGet, HttpOptions]
        [Route("{keyword}")]
        public async Task<ActionResult<MobileViewModel>> Details(string culture, string keyword)
        {
            string msg = string.Empty;
            Expression<Func<MixConfiguration, bool>> predicate = null;
            predicate = m => m.Keyword == keyword;
            var readResult = await base.GetSingleAsync<MobileViewModel>(keyword, predicate, null);
            return Ok(readResult.Data?.Value);
        }

        // GET api/Configurations/keyword
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [EnableQuery(MaxExpansionDepth = 4)]
        [HttpGet, HttpOptions]
        [Route("type/{type}")]
        public async Task<ActionResult<JObject>> ListByType(string culture, string type, ODataQueryOptions<MixConfiguration> queryOptions)
        {
            Expression<Func<MixConfiguration, bool>> predicate = m => m.Category == type && m.Specificulture == culture;
            var data = await base.GetListAsync<MobileViewModel>(predicate, $"type_{type}", queryOptions);
            JObject result = new JObject();
            foreach (var item in data)
            {
                result.Add(new JProperty(item.Keyword, item.Value));
            }
            return Ok(result);
        }

        // GET api/Configurations/keyword
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [EnableQuery(MaxExpansionDepth = 4)]
        [HttpGet, HttpOptions]
        public async Task<ActionResult<JObject>> List(string culture, ODataQueryOptions<MixConfiguration> queryOptions)
        {
            var data = await base.GetListAsync<MobileViewModel>(queryOptions);
            JObject result = new JObject();
            foreach (var item in data)
            {
                result.Add(new JProperty(item.Keyword, item.Value));
            }
            return Ok(result);
        }

        // GET api/attribute-sets/read/count
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [EnableQuery]
        [Route("count")]
        [HttpGet, HttpOptions]
        public async System.Threading.Tasks.Task<ActionResult<int>> CountAsync()
        {
            return (await MobileViewModel.Repository.CountAsync()).Data;
        }

        // Save api/odata/{culture}/attribute-set/read/{keyword}
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin, Admin")]
        [HttpPost, HttpOptions]
        [Route("{keyword}")]
        public async Task<ActionResult<MobileViewModel>> Save(string culture, string keyword, [FromBody]JObject data)
        {
            Expression<Func<MixConfiguration, bool>> predicate = model => model.Keyword == keyword && model.Specificulture == culture;
            var getData = await base.GetSingleAsync<MobileViewModel>(keyword, predicate, null);

            if (getData.IsSucceed)
            {
                getData.Data.Value = data["value"].Value<string>();
                var saveResult = await getData.Data.SaveModelAsync(true);
                if (saveResult.IsSucceed)
                {
                    return Ok(saveResult);
                }
                else
                {
                    return BadRequest(saveResult);
                }
            }
            else
            {
                return BadRequest(getData);
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin, Admin")]
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