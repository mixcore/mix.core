// Licensed to the Mix I/O Foundation under one or more agreements.
// The Mix I/O Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using Mix.Domain.Core.ViewModels;
using Mix.Cms.Lib.Models.Cms;
using System.Linq.Expressions;
using Mix.Common.Helper;
using Microsoft.AspNetCore.Http;
using Mix.Cms.Lib.ViewModels.MixMedias;
using Microsoft.Extensions.Caching.Memory;
using static Mix.Cms.Lib.MixEnums;
using System.Web;

namespace Mix.Cms.Api.Controllers.v1
{
    [Route("api/v1/{culture}/media")]
    public class ApiMediaController :
      BaseGenericApiController<MixCmsContext, MixMedia>
    {
        public ApiMediaController(IMemoryCache memoryCache, Microsoft.AspNetCore.SignalR.IHubContext<Hub.PortalHub> hubContext) : base(memoryCache, hubContext)
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin, Admin")]
        [HttpPost, HttpOptions]
        [Route("save")]
        public async Task<RepositoryResponse<UpdateViewModel>> Save([FromBody]UpdateViewModel model)
        {
            if (model != null)
            {
                var result = await base.SaveAsync<UpdateViewModel>(model, true);
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
                        && (!request.Status.HasValue || model.Status == request.Status.Value)
                        && (string.IsNullOrWhiteSpace(request.Keyword)
                            || (model.Title.Contains(request.Keyword)
                            || model.Description.Contains(request.Keyword)))
                        && (!request.FromDate.HasValue
                            || (model.CreatedDateTime >= request.FromDate.Value)
                        )
                        && (!request.ToDate.HasValue
                            || (model.CreatedDateTime <= request.ToDate.Value)
                        );
            string key = $"{request.Key}_{request.PageSize}_{request.PageIndex}";
            switch (request.Key)
            {
                
                default:
                    var portalResult = await base.GetListAsync<UpdateViewModel>(key, request, predicate);
                    
                    return Ok(JObject.FromObject(portalResult));
                
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
        protected async Task<string> UploadFileAsync(IFormFile file, string folderPath)
        {
            if (file?.Length > 0)
            {
                string fileName = await CommonHelper.UploadFileAsync(folderPath, file).ConfigureAwait(false);
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


        #endregion
    }
}
