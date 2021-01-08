﻿// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Mix.Cms.Lib;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Cms.Lib.ViewModels.MixTemplates;
using Mix.Domain.Core.ViewModels;
using Newtonsoft.Json.Linq;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Constants;
namespace Mix.Cms.Api.Controllers.v1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "SuperAdmin, Admin")]
    [Produces("application/json")]
    [Route("api/v1/{culture}/template")]
    public class ApiTemplateController :
            BaseGenericApiController<MixCmsContext, MixTemplate>
    {
        public ApiTemplateController(MixCmsContext context, IMemoryCache memoryCache, Microsoft.AspNetCore.SignalR.IHubContext<Mix.Cms.Service.SignalR.Hubs.PortalHub> hubContext) : base(context, memoryCache, hubContext)
        {
        }

        #region Get

        [HttpGet, HttpOptions]
        [Route("details/{viewType}/{themeId}/{folderType}/{id}")]
        [Route("details/{viewType}/{themeId}/{folderType}")]
        public async Task<ActionResult<RepositoryResponse<UpdateViewModel>>> DetailsAsync(string viewType, int themeId, MixTemplateFolderType folderType, int? id)
        {
            if (id.HasValue)
            {
                Expression<Func<MixTemplate, bool>> predicate = model => model.Id == id;
                var portalResult = await base.GetSingleAsync<UpdateViewModel>($"{viewType}_{themeId}_{folderType}_{id}", predicate);
                return Ok(JObject.FromObject(portalResult));
            }
            else
            {
                var getTheme = await Lib.ViewModels.MixThemes.ReadViewModel.Repository.GetSingleModelAsync(t => t.Id == themeId);
                if (getTheme.IsSucceed)
                {
                    var model = new MixTemplate()
                    {
                        Status = MixService.GetConfig<MixContentStatus>(AppSettingKeywords.DefaultContentStatus),
                        ThemeId = themeId,
                        ThemeName = getTheme.Data.Name,
                        Extension = MixService.GetConfig<string>("TemplateExtension"),
                        FolderType = folderType
                    };

                    RepositoryResponse<UpdateViewModel> result = await base.GetSingleAsync<UpdateViewModel>($"{viewType}_default", null, model);
                    return Ok(JObject.FromObject(result));
                }
                else
                {
                    return new RepositoryResponse<Lib.ViewModels.MixTemplates.UpdateViewModel>();
                }
            }
        }

        // GET api/category/id
        [HttpGet, HttpOptions]
        [Route("delete/{id}")]
        public async Task<RepositoryResponse<MixTemplate>> DeleteAsync(int id)
        {
            return await base.DeleteAsync<UpdateViewModel>(
                model => model.Id == id, true);
        }

        #endregion Get

        #region Post

        // POST api/template
        [HttpPost, HttpOptions]
        [Route("save")]
        public async Task<RepositoryResponse<Lib.ViewModels.MixTemplates.UpdateViewModel>> Save(
            [FromBody] Lib.ViewModels.MixTemplates.UpdateViewModel model)
        {
            return await base.SaveAsync<UpdateViewModel>(model, true);
        }

        // GET api/template
        [HttpPost, HttpOptions]
        [Route("list/{themeId}")]
        public async Task<ActionResult<JObject>> GetList(
            int themeId,
            [FromBody] RequestPaging request
            )
        {
            bool isFolderType = Enum.TryParse(Request.Query["folderType"], out MixTemplateFolderType folderType);
            Expression<Func<MixTemplate, bool>> predicate = model =>
                model.ThemeId == themeId
                 && (!isFolderType
                    ||
                    (
                        model.FolderType == folderType
                    ))
                && (string.IsNullOrWhiteSpace(request.Keyword)
                    ||
                    (
                         model.FileName.Contains(request.Keyword)
                        || model.FileFolder.Contains(request.Keyword)
                    ));

            switch (request.Key)
            {
                case "mvc":
                    var mvcResult = await base.GetListAsync<ReadViewModel>(request, predicate);
                    return Ok(JObject.FromObject(mvcResult));

                case "portal":
                    var portalResult = await base.GetListAsync<UpdateViewModel>(request, predicate);
                    return Ok(JObject.FromObject(portalResult));

                default:
                    var listItemResult = await base.GetListAsync<UpdateViewModel>(request, predicate);

                    return JObject.FromObject(listItemResult);
            }
        }

        #endregion Post
    }
}