// Licensed to the Mix I/O Foundation under one or more agreements.
// The Mix I/O Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Mix.Domain.Core.ViewModels;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using System.Linq.Expressions;
using Mix.Cms.Lib.ViewModels.MixThemes;
using Microsoft.AspNetCore.SignalR;
using Mix.Cms.Hub;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using Mix.Cms.Lib;
using Mix.Cms.Lib.Repositories;
using Mix.Cms.Lib.Attributes;
using Microsoft.AspNetCore.Http;
using Mix.Domain.Data.Repository;
using Mix.Cms.Lib.ViewModels;

namespace Mix.Cms.Api.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1/{culture}/theme")]
    public class ApiThemeController :
        BaseGenericApiController<MixCmsContext, MixTheme>
    {
        public ApiThemeController(IMemoryCache memoryCache, IHubContext<PortalHub> hubContext) : base(memoryCache, hubContext)
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
        public async Task<RepositoryResponse<string>> Export(int id, [FromBody]SiteStructureViewModel data)
        {
            var getTemplate = await ReadViewModel.Repository.GetSingleModelAsync(
                 theme => theme.Id == id).ConfigureAwait(false);
            
            //path to temporary folder
            string tempPath = $"wwwroot/Exports/Themes/{getTemplate.Data.Name}/temp";
            string outputPath = $"Exports/Themes/{getTemplate.Data.Name}";

            string filename = $"schema";
            var file = new FileViewModel()
            {
                Filename = filename,
                Extension = ".json",
                FileFolder = $"{tempPath}/Data",
                Content = JObject.FromObject(data).ToString()
            };

            // Delete Existing folder
            FileRepository.Instance.DeleteFolder(outputPath);
            // Copy current templates file
            FileRepository.Instance.CopyDirectory($"{getTemplate.Data.TemplateFolder}", $"{tempPath}/Templates");
            // Copy current assets files
            FileRepository.Instance.CopyDirectory($"wwwroot/{getTemplate.Data.AssetFolder}", $"{tempPath}/Assets");            
            // Copy current uploads files
            FileRepository.Instance.CopyDirectory($"wwwroot/{getTemplate.Data.UploadsFolder}", $"{tempPath}/Uploads");
            // Save Site Structures
            FileRepository.Instance.SaveFile(file);

            // Zip to [theme_name].zip ( wwwroot for web path)
            string filePath = FileRepository.Instance.ZipFolder($"{tempPath}", outputPath, getTemplate.Data.Name);

            

            // Delete temp folder
            FileRepository.Instance.DeleteWebFolder($"{outputPath}/Assets");
            FileRepository.Instance.DeleteWebFolder($"{outputPath}/Uploads");
            FileRepository.Instance.DeleteWebFolder($"{outputPath}/Templates");
            FileRepository.Instance.DeleteWebFolder($"{outputPath}/Data");

            return new RepositoryResponse<string>()
            {
                IsSucceed = !string.IsNullOrEmpty(outputPath),
                Data = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/{filePath}"
            };
        }

        // GET api/theme/id
        [HttpGet, HttpOptions]
        [Route("delete/{id}")]
        public async Task<RepositoryResponse<MixTheme>> DeleteAsync(int id)
        {
            return await base.DeleteAsync<UpdateViewModel>(
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
                            Status = MixService.GetConfig<int>("DefaultStatus")
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
                            Status = MixService.GetConfig<int>("DefaultStatus")
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
        [RequestFormSizeLimit(valueCountLimit: 214748364)] // 200Mb
        [Route("save")]
        public async Task<RepositoryResponse<UpdateViewModel>> Save([FromForm]string model, [FromForm]IFormFile assets, [FromForm]IFormFile theme)
        {
            var json = JObject.Parse(model);
            var data = json.ToObject<UpdateViewModel>();
            if (assets!=null)
            {
                data.Asset = new Lib.ViewModels.FileViewModel(assets, data.AssetFolder);
                FileRepository.Instance.SaveWebFile(assets, data.AssetFolder);
            }
            if (theme!=null)
            {
                string importFolder = $"Imports/Themes/{DateTime.UtcNow.ToString("dd-MM-yyyy")}/{data.Name}";
                data.TemplateAsset = new Lib.ViewModels.FileViewModel(theme, importFolder);
                FileRepository.Instance.SaveWebFile(theme, importFolder);
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
            string key = $"{request.Key}_{request.PageSize}_{request.PageIndex}";
            switch (request.Key)
            {
                case "portal":
                    var portalResult = await base.GetListAsync<UpdateViewModel>(key, request, predicate);
                    return Ok(JObject.FromObject(portalResult));
                default:

                    var listItemResult = await base.GetListAsync<ReadViewModel>(key, request, predicate);

                    return JObject.FromObject(listItemResult);
            }
        }

        #endregion Post
    }
}
