﻿// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Mix.Cms.Lib;
using Mix.Cms.Lib.Attributes;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Repositories;
using Mix.Cms.Lib.Services;
using Mix.Cms.Lib.ViewModels;
using Mix.Cms.Lib.ViewModels.MixThemes;
using Mix.Domain.Core.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mix.Cms.Api.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1/{culture}/theme")]
    public class ApiThemeController :
        BaseGenericApiController<MixCmsContext, MixTheme>
    {
        public ApiThemeController(MixCmsContext context, IMemoryCache memoryCache, Microsoft.AspNetCore.SignalR.IHubContext<Mix.Cms.Service.SignalR.Hubs.PortalHub> hubContext) : base(context, memoryCache, hubContext)
        {
        }

        #region Get

        // GET api/theme/id
        [HttpGet, HttpOptions]
        [Route("sync/{id}")]
        public async Task<RepositoryResponse<List<Lib.ViewModels.MixTemplates.UpdateViewModel>>> Sync(int id)
        {
            var getTemplate = await Lib.ViewModels.MixTemplates.UpdateViewModel.Repository.GetModelListByAsync(
                 template => template.ThemeId == id).ConfigureAwait(false);
            foreach (var item in getTemplate.Data)
            {
                await item.SaveModelAsync(true).ConfigureAwait(false);
            }
            return getTemplate;
        }

        // GET api/theme/id
        [HttpGet, HttpOptions]
        [Route("export/{id}")]
        public async Task<RepositoryResponse<SiteStructureViewModel>> Export(int id)
        {
            var siteStructures = new SiteStructureViewModel();
            await siteStructures.InitAsync(_lang);
            return new RepositoryResponse<SiteStructureViewModel>()
            {
                IsSucceed = true,
                Data = siteStructures
            };
        }

        // GET api/theme/id
        [HttpPost, HttpOptions]
        [Route("export/{id}")]
        public Task<RepositoryResponse<string>> Export(int id, [FromBody] SiteStructureViewModel data)
        {
            return Lib.ViewModels.MixThemes.Helper.ExportTheme(id, data
                , _lang, HttpContext.Request.Scheme, HttpContext.Request.Host.Value);
        }

        // GET api/theme/id
        [HttpGet, HttpOptions]
        [Route("delete/{id}")]
        public async Task<RepositoryResponse<MixTheme>> DeleteAsync(int id)
        {
            return await base.DeleteAsync<DeleteViewModel>(
                model => model.Id == id, true);
        }

        // GET api/themes/id
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
                        Expression<Func<MixTheme, bool>> predicate = model => model.Id == id;
                        var portalResult = await base.GetSingleAsync<UpdateViewModel>($"{viewType}_{id}", predicate);
                        if (portalResult.IsSucceed)
                        {
                            portalResult.Data.IsActived = MixService.GetConfig<int>(MixConstants.ConfigurationKeyword.ThemeId, _lang) == portalResult.Data.Id;
                        }
                        return Ok(JObject.FromObject(portalResult));
                    }
                    else
                    {
                        var model = new MixTheme()
                        {
                            Status = Enum.Parse<MixEnums.MixContentStatus>(MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.DefaultContentStatus))
                            ,
                            Priority = UpdateViewModel.Repository.Max(a => a.Priority).Data + 1
                        };

                        RepositoryResponse<UpdateViewModel> result = await base.GetSingleAsync<UpdateViewModel>($"{viewType}_default", null, model);
                        return Ok(JObject.FromObject(result));
                    }
                default:
                    if (id.HasValue)
                    {
                        Expression<Func<MixTheme, bool>> predicate = model => model.Id == id;
                        var result = await base.GetSingleAsync<ReadViewModel>($"{viewType}_{id}", predicate);
                        if (result.IsSucceed)
                        {
                            result.Data.IsActived = MixService.GetConfig<int>(MixConstants.ConfigurationKeyword.ThemeId, _lang) == result.Data.Id;
                        }
                        return Ok(JObject.FromObject(result));
                    }
                    else
                    {
                        var model = new MixTheme()
                        {
                            Status = MixService.GetConfig<MixEnums.MixContentStatus>(MixConstants.ConfigurationKeyword.DefaultContentStatus)
                            ,
                            Priority = ReadViewModel.Repository.Max(a => a.Priority).Data + 1
                        };

                        RepositoryResponse<ReadViewModel> result = await base.GetSingleAsync<ReadViewModel>($"{viewType}_default", null, model);
                        return Ok(JObject.FromObject(result));
                    }
            }
        }

        #endregion Get

        #region Post

        // POST api/theme
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin, Admin")]
        [HttpPost, HttpOptions]
        [DisableRequestSizeLimit]
        [Route("save")]
        public async Task<RepositoryResponse<UpdateViewModel>> Save([FromForm] string model, [FromForm] IFormFile assets, [FromForm] IFormFile theme)
        {
            var data = JsonConvert.DeserializeObject<UpdateViewModel>(model);

            if (assets != null)
            {
                data.Asset = new Lib.ViewModels.FileViewModel(assets, data.AssetFolder);
                FileRepository.Instance.SaveFile(assets, assets.FileName, $"wwwroot/{data.AssetFolder}");
            }
            if (theme != null)
            {
                string importFolder = $"Imports/Themes/{DateTime.UtcNow.ToString("dd-MM-yyyy")}/{data.Name}";
                data.TemplateAsset = new Lib.ViewModels.FileViewModel(theme, importFolder);
                FileRepository.Instance.SaveFile(theme, theme.FileName, $"wwwroot/{importFolder}");
            }

            // Load default blank if created new without upload theme
            if (data.Id == 0 && theme == null)
            {
                if (data.IsCloneFromCurrentTheme)
                {
                    var currentThemeFolder = $"{MixConstants.Folder.TemplatesFolder}/{MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.ThemeFolder, _lang)}";
                    var assetFolder = $"{MixConstants.Folder.FileFolder}/{MixConstants.Folder.TemplatesAssetFolder}/{MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.ThemeFolder, _lang)}/assets";
                    FileRepository.Instance.CopyDirectory(currentThemeFolder, data.TemplateFolder);
                    FileRepository.Instance.CopyDirectory(assetFolder, $"wwwroot/{data.AssetFolder}");
                }
                else
                {
                    data.TemplateAsset = new Lib.ViewModels.FileViewModel()
                    {
                        Filename = "default_blank",
                        Extension = ".zip",
                        FileFolder = "Imports/Themes"
                    };
                }
            }

            if (data != null)
            {
                data.CreatedBy = User.Claims.FirstOrDefault(c => c.Type == "Username")?.Value;
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

        // GET api/theme
        [HttpPost, HttpOptions]
        [Route("list")]
        public async Task<ActionResult<JObject>> GetList(
            [FromBody] RequestPaging request)
        {
            ParseRequestPagingDate(request);
            Expression<Func<MixTheme, bool>> predicate = model =>
                string.IsNullOrWhiteSpace(request.Keyword)
                    || (model.Name.Contains(request.Keyword)
                    )
                && (!request.FromDate.HasValue
                    || (model.CreatedDateTime >= request.FromDate.Value)
                )
                && (!request.ToDate.HasValue
                    || (model.CreatedDateTime <= request.ToDate.Value)
                )
                    ;
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