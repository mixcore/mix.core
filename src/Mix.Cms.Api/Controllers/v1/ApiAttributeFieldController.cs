﻿// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Mix.Cms.Lib;
using Mix.Cms.Lib.Attributes;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Cms.Lib.ViewModels.MixAttributeFields;
using Mix.Domain.Core.ViewModels;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Constants;
namespace Mix.Cms.Api.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1/{culture}/attribute-field")]
    public class ApiAttributeFieldController :
        BaseGenericApiController<MixCmsContext, MixAttributeField>
    {
        public ApiAttributeFieldController(MixCmsContext context, IMemoryCache memoryCache, Microsoft.AspNetCore.SignalR.IHubContext<Mix.Cms.Service.SignalR.Hubs.PortalHub> hubContext) : base(context, memoryCache, hubContext)
        {
        }

        #region Get

        // GET api/attribute-field/id
        [HttpGet, HttpOptions]
        [Route("delete/{id}")]
        public async Task<RepositoryResponse<MixAttributeField>> DeleteAsync(int id)
        {
            return await base.DeleteAsync<UpdateViewModel>(
                model => model.Id == id, true);
        }

        // GET api/attribute-fields/id
        [HttpGet, HttpOptions]
        [Route("details/{id}/{viewType}")]
        [Route("details/{viewType}")]
        public async Task<ActionResult<JObject>> Details(string viewType, int? id)
        {
            string msg = string.Empty;
            switch (viewType)
            {
                case "portal":
                    if (id.HasValue)
                    {
                        Expression<Func<MixAttributeField, bool>> predicate = model => model.Id == id;
                        var portalResult = await base.GetSingleAsync<UpdateViewModel>($"{viewType}_{id}", predicate);
                        return Ok(JObject.FromObject(portalResult));
                    }
                    else
                    {
                        var model = new MixAttributeField()
                        {
                            Status = MixService.GetConfig<MixContentStatus>(AppSettingKeywords.DefaultContentStatus)
                            ,
                            Priority = UpdateViewModel.Repository.Max(a => a.Priority).Data + 1
                        };

                        RepositoryResponse<UpdateViewModel> result = await base.GetSingleAsync<UpdateViewModel>($"{viewType}_default", null, model);
                        return Ok(JObject.FromObject(result));
                    }
                default:
                    if (id.HasValue)
                    {
                        Expression<Func<MixAttributeField, bool>> predicate = model => model.Id == id;
                        var result = await base.GetSingleAsync<ReadViewModel>($"{viewType}_{id}", predicate);
                        return Ok(JObject.FromObject(result));
                    }
                    else
                    {
                        var model = new MixAttributeField()
                        {
                            Status = MixService.GetConfig<MixContentStatus>(AppSettingKeywords.DefaultContentStatus)
                            ,
                            Priority = ReadViewModel.Repository.Max(a => a.Priority).Data + 1
                        };

                        RepositoryResponse<ReadViewModel> result = await base.GetSingleAsync<ReadViewModel>($"{viewType}_default", null, model);
                        return Ok(JObject.FromObject(result));
                    }
            }
        }

        // GET api/module-data/create/id
        [AllowAnonymous]
        [HttpGet, HttpOptions]
        [Route("init-form/{setId}")]
        public async Task<RepositoryResponse<List<UpdateViewModel>>> InitByName(int setId)
        {
            return await UpdateViewModel.Repository.GetModelListByAsync(
                m => m.AttributeSetId == setId).ConfigureAwait(false);
        }

        #endregion Get

        #region Post

        // POST api/attribute-field
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin, Admin")]
        [HttpPost, HttpOptions]
        [RequestFormSizeLimit(valueCountLimit: 214748364)] // 200Mb
        [Route("save")]
        public async Task<RepositoryResponse<UpdateViewModel>> Save(UpdateViewModel data)
        {
            if (data != null)
            {
                data.Specificulture = _lang;
                var result = await base.SaveAsync<UpdateViewModel>(data, true);
                if (result.IsSucceed)
                {
                    MixService.LoadFromDatabase();
                    MixService.SaveSettings();
                }
                return result;
            }
            return new RepositoryResponse<UpdateViewModel>() { Status = 501 };
        }

        // GET api/attribute-field
        [HttpPost, HttpOptions]
        [Route("list")]
        public async Task<ActionResult<JObject>> GetList(
            [FromBody] RequestPaging request)
        {
            ParseRequestPagingDate(request);
            Expression<Func<MixAttributeField, bool>> predicate = model =>
                (string.IsNullOrWhiteSpace(request.Keyword)
                    || (EF.Functions.Like(model.Name, $"%{request.Keyword}%"))
                    );
            switch (request.Key)
            {
                case "portal":
                    var portalResult = await base.GetListAsync<UpdateViewModel>(request, predicate);
                    return Ok(JObject.FromObject(portalResult));

                default:

                    var listItemResult = await base.GetListAsync<ReadViewModel>(request, predicate);

                    return JObject.FromObject(listItemResult);
            }
        }

        #endregion Post
    }
}