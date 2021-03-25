// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mix.Cms.Lib.Controllers;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Models.Common;
using Mix.Cms.Lib.ViewModels;
using Mix.Cms.Lib.ViewModels.MixThemes;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Mix.Heart.Extensions;
using Mix.Cms.Lib.Services;
using Newtonsoft.Json.Linq;
using Mix.Services;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Mix.Cms.Service.SignalR.Hubs;

namespace Mix.Cms.Api.RestFul.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1/rest/theme/portal")]
    [Route("api/v1/rest/{culture}/theme/portal")]
    public class ApiThemeController :
        BaseAuthorizedRestApiController<MixCmsContext, MixTheme, UpdateViewModel, ReadViewModel, DeleteViewModel>
    {
        protected readonly IHubContext<PortalHub> _hubContext;
        HttpService _httpService;
        public ApiThemeController(
            HttpService httpService,
            DefaultRepository<MixCmsContext, MixTheme, ReadViewModel> repo,
            DefaultRepository<MixCmsContext, MixTheme, UpdateViewModel> updRepo,
            DefaultRepository<MixCmsContext, MixTheme, DeleteViewModel> delRepo,
            IHubContext<PortalHub> hubContext) : base(repo, updRepo, delRepo)
        {
            _httpService = httpService;
            _hubContext = hubContext;
        }

        [HttpGet]
        public override async Task<ActionResult<PaginationModel<ReadViewModel>>> Get()
        {
            var searchQuery = new SearchQueryModel(Request);
            Expression<Func<MixTheme, bool>> predicate = null;
            predicate = predicate.AndAlsoIf(searchQuery.Status.HasValue, model => model.Status == searchQuery.Status.Value);
            predicate = predicate.AndAlsoIf(searchQuery.FromDate.HasValue, model => model.CreatedDateTime >= searchQuery.FromDate.Value);
            predicate = predicate.AndAlsoIf(searchQuery.ToDate.HasValue, model => model.CreatedDateTime <= searchQuery.ToDate.Value);
            predicate = predicate.AndAlsoIf(!string.IsNullOrEmpty(searchQuery.Keyword),
                model => EF.Functions.Like(model.Name, $"%{searchQuery.Keyword}%"));
            var getData = await GetListAsync<ReadViewModel>(predicate, searchQuery);
            if (getData.IsSucceed)
            {
                return getData.Data;
            }
            else
            {
                return BadRequest(getData.Errors);
            }
        }

        [HttpGet("{id}")]
        public override async Task<ActionResult<UpdateViewModel>> Get(string id)
        {
            RepositoryResponse<UpdateViewModel> getTheme = await _updRepo.GetSingleModelAsync(m => m.Id == int.Parse(id));
            if (getTheme.IsSucceed)
            {
                getTheme.Data.IsActived = MixService.GetConfig<int>(MixAppSettingKeywords.ThemeId, _lang) == getTheme.Data.Id;
                return Ok(getTheme.Data);
            }
            else
            {
                return BadRequest(getTheme.Errors);
            }
        }

        // POST api/theme
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin, Admin")]
        [HttpPost, HttpOptions]
        [DisableRequestSizeLimit]
        [Route("save")]
        public async Task<RepositoryResponse<UpdateViewModel>> Save(
            [FromForm] string model, [FromForm] IFormFile assets, [FromForm] IFormFile theme)
        {
            var data = JsonConvert.DeserializeObject<UpdateViewModel>(model);

            if (assets != null)
            {
                data.Asset = new FileViewModel(assets, data.AssetFolder);
                MixFileRepository.Instance.SaveFile(assets, $"wwwroot/{data.AssetFolder}");
            }
            if (theme != null)
            {
                string importFolder = $"Imports/Themes/{DateTime.UtcNow.ToString("dd-MM-yyyy")}/{data.Name}";
                data.TemplateAsset = new FileViewModel(theme, importFolder);
                MixFileRepository.Instance.SaveFile(theme, $"wwwroot/{importFolder}");
            }

            // Load default blank if created new without upload theme
            if (data.Id == 0 && theme == null)
            {
                if (data.IsCloneFromCurrentTheme)
                {
                    var currentThemeFolder = $"{MixFolders.TemplatesFolder}/{MixService.GetConfig<string>(MixAppSettingKeywords.ThemeFolder, _lang)}";
                    var assetFolder = $"{MixFolders.SiteContentAssetsFolder}/{MixService.GetConfig<string>(MixAppSettingKeywords.ThemeFolder, _lang)}/assets";
                    MixFileRepository.Instance.CopyDirectory(currentThemeFolder, data.TemplateFolder);
                    MixFileRepository.Instance.CopyDirectory(assetFolder, $"wwwroot/{data.AssetFolder}");
                }
                else
                {
                    data.TemplateAsset = new FileViewModel()
                    {
                        Filename = "default_blank",
                        Extension = MixFileExtensions.Zip,
                        FileFolder = MixFolders.ImportFolder
                    };
                }
            }

            if (data != null)
            {
                data.CreatedBy = IdentityHelper.GetClaim(User, MixClaims.Username);
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


        [HttpPost]
        [Route("install")]
        public async Task<ActionResult<RepositoryResponse<UpdateViewModel>>> InstallTheme([FromBody] JObject theme)
        {
            var progress = new Progress<double>();
            progress.ProgressChanged += (sender, value) =>
            {
                _ = AlertAsync(_hubContext.Clients.Group("Theme"), "Downloading", 200, value);
            };

            string createdBy = IdentityHelper.GetClaim(User, MixClaims.Username);
            var result = await Helper.InstallThemeAsync(theme, createdBy, _lang, progress, _httpService);
            if (result.IsSucceed)
            {
                return Ok(result.Data);
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        protected async Task AlertAsync<T>(IClientProxy clients, string action, int status, T message)
        {
            var address = Request.Headers["X-Forwarded-For"];
            if (string.IsNullOrEmpty(address))
            {
                address = Request.Host.Value;
            }
            var logMsg = new JObject()
                {
                    new JProperty("created_at", DateTime.UtcNow),
                    new JProperty("id", Request.HttpContext.Connection.Id.ToString()),
                    new JProperty("address", address),
                    new JProperty("ip_address", Request.HttpContext.Connection.RemoteIpAddress.ToString()),
                    new JProperty("user", IdentityHelper.GetClaim(User, MixClaims.Username)),
                    new JProperty("request_url", Request.Path.Value),
                    new JProperty("action", action),
                    new JProperty("status", status),
                    new JProperty("message", message)
                };

            //It's not possible to configure JSON serialization in the JavaScript client at this time (March 25th 2020).
            //https://docs.microsoft.com/en-us/aspnet/core/signalr/configuration?view=aspnetcore-3.1&tabs=dotnet
            await clients.SendAsync(
                Service.SignalR.Constants.HubMethods.ReceiveMethod, logMsg.ToString(Formatting.None));
        }



        // GET api/theme/id
        [HttpGet]
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
        [HttpGet]
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
        [HttpPost]
        [Route("export/{id}")]
        public Task<RepositoryResponse<string>> Export(int id, [FromBody] SiteStructureViewModel data)
        {
            return Helper.ExportTheme(id, data
                , _lang, HttpContext.Request.Scheme, HttpContext.Request.Host.Value);
        }
    }
}