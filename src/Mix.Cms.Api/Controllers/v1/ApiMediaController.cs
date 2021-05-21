// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Cms.Lib.SignalR.Hubs;
using Mix.Cms.Lib.ViewModels.MixMedias;
using Mix.Common.Helper;
using Mix.Heart.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;

namespace Mix.Cms.Api.Controllers.v1
{
    [Route("api/v1/{culture}/media")]
    public class ApiMediaController :
      BaseGenericApiController<MixCmsContext, MixMedia>
    {
        public ApiMediaController(MixCmsContext context, IMemoryCache memoryCache, Microsoft.AspNetCore.SignalR.IHubContext<PortalHub> hubContext) : base(context, memoryCache, hubContext)
        {
        }

        #region Get

        // GET api/media/id
        [HttpGet, HttpOptions]
        [Route("delete/{id}")]
        public async Task<RepositoryResponse<MixMedia>> DeleteAsync(int id)
        {
            return await base.DeleteAsync<UpdateViewModel>(
                model => model.Id == id && model.Specificulture == _lang, true);
        }

        // GET api/medias/id
        [HttpGet, HttpOptions]
        [Route("details/{id}/{viewType}")]
        [Route("details/{viewType}")]
        public async Task<ActionResult<JObject>> Details(string viewType, int? id)
        {
            string msg = string.Empty;
            switch (viewType)
            {
                default:
                    if (id.HasValue)
                    {
                        var beResult = await UpdateViewModel.Repository.GetSingleModelAsync(model => model.Id == id && model.Specificulture == _lang).ConfigureAwait(false);
                        return Ok(JObject.FromObject(beResult));
                    }
                    else
                    {
                        var model = new MixMedia();
                        RepositoryResponse<UpdateViewModel> result = new RepositoryResponse<UpdateViewModel>()
                        {
                            IsSucceed = true,
                            Data = new UpdateViewModel(model)
                            {
                                Specificulture = _lang,
                                Status = MixContentStatus.Published,
                            }
                        };
                        return Ok(JObject.FromObject(result));
                    }
            }
        }

        #endregion Get

        #region Post

        // POST api/media
        [HttpPost, HttpOptions]
        [Route("save")]
        public async Task<RepositoryResponse<UpdateViewModel>> Save([FromForm] string model, [FromForm] IFormFile file)
        {
            if (model != null)
            {
                var json = JObject.Parse(model);
                var data = json.ToObject<UpdateViewModel>();
                data.Status = MixService.GetEnumConfig<MixContentStatus>(MixAppSettingKeywords.DefaultContentStatus);
                data.Specificulture = _lang;
                data.File = file;
                var result = await base.SaveAsync<UpdateViewModel>(data, true);
                return result;
            }
            return new RepositoryResponse<UpdateViewModel>() { Status = 501 };
        }

        [DisableRequestSizeLimit]
        [HttpPost]
        [Route("upload-media")]
        public async Task<RepositoryResponse<UpdateViewModel>> UploadMedia([FromForm] IFormFile file)
        {
            if (file != null)
            {
                var data = new UpdateViewModel()
                {
                    Status = MixService.GetEnumConfig<MixContentStatus>(MixAppSettingKeywords.DefaultContentStatus),
                    Specificulture = _lang,
                    FileFolder = $"{MixService.GetTemplateUploadFolder(_lang)}",
                    File = file
                };
                var result = await base.SaveAsync<UpdateViewModel>(data, true);
                return result;
            }
            return new RepositoryResponse<UpdateViewModel>() { Status = 501 };
        }

        // GET api/media
        [HttpPost, HttpOptions]
        [Route("list")]
        public async Task<ActionResult<JObject>> GetList(
            [FromBody] RequestPaging request)
        {
            var parsed = HttpUtility.ParseQueryString(request.Query ?? "");
            bool isLevel = int.TryParse(parsed.Get("level"), out int level);
            ParseRequestPagingDate(request);
            Expression<Func<MixMedia, bool>> predicate = model =>
                        model.Specificulture == _lang
                        && (string.IsNullOrEmpty(request.Status) || model.Status == Enum.Parse<MixContentStatus>(request.Status))
                        && (string.IsNullOrWhiteSpace(request.Keyword)
                            || (model.Title.Contains(request.Keyword)
                            || model.Description.Contains(request.Keyword)))
                        && (!request.FromDate.HasValue
                            || (model.CreatedDateTime >= request.FromDate.Value)
                        )
                        && (!request.ToDate.HasValue
                            || (model.CreatedDateTime <= request.ToDate.Value)
                        );
            switch (request.Key)
            {
                default:
                    var portalResult = await base.GetListAsync<ReadViewModel>(request, predicate);

                    return Ok(JObject.FromObject(portalResult));
            }
        }

        // POST api/update-infos
        [HttpPost, HttpOptions]
        [Route("update-infos")]
        public async Task<RepositoryResponse<List<UpdateViewModel>>> UpdateInfos([FromBody] List<UpdateViewModel> models)
        {
            if (models != null)
            {
                return await base.SaveListAsync(models, false);
            }
            else
            {
                return new RepositoryResponse<List<UpdateViewModel>>();
            }
        }

        #endregion Post

        #region Helpers

        /// <summary>
        /// Uploads the file asynchronous.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="folderPath">The folder path.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("media/upload")]
        protected async Task<string> UploadFileAsync(IFormFile file, string folderPath)
        {
            if (file?.Length > 0)
            {
                string fileName = await MixCommonHelper.UploadFileAsync(folderPath, file).ConfigureAwait(false);
                if (!string.IsNullOrEmpty(fileName))
                {
                    string filePath = string.Format("{0}/{1}", folderPath, fileName);
                    return filePath;
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }
        }

        #endregion Helpers
    }
}