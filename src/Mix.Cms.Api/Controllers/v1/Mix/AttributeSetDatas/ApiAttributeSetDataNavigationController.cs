// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Mix.Cms.Lib;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Cms.Lib.ViewModels;
using Mix.Cms.Lib.ViewModels.MixAttributeSetDatas;
using Mix.Domain.Core.ViewModels;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using static Mix.Cms.Lib.MixEnums;

namespace Mix.Cms.Api.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1/{culture}/navigations")]
    public class ApiAttributeSetDataNavigationController :
        BaseGenericApiController<MixCmsContext, MixAttributeSetData>
    {
        public ApiAttributeSetDataNavigationController(MixCmsContext context, IMemoryCache memoryCache, Microsoft.AspNetCore.SignalR.IHubContext<Hub.PortalHub> hubContext) : base(context, memoryCache, hubContext)
        {
        }

        #region Get

        // GET api/attribute-set-datas/id
        [HttpGet, HttpOptions]
        [Route("{name}")]
        public async Task<ActionResult<JObject>> Details(string name)
        {
            var result = await MixCmsHelper.GetNavigation(name, _lang, Url);
            return Ok(result);
        }

        #endregion Get
    }
}