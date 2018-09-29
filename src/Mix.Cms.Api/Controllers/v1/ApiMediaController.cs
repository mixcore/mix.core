// Licensed to the Mix I/O Foundation under one or more agreements.
// The Mix I/O Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Mix.Cms.Lib.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using Mix.Domain.Core.ViewModels;
using Mix.Cms.Lib;
using Mix.Cms.Lib.Models.Cms;
using System.Linq.Expressions;
using Mix.Common.Helper;
using Microsoft.AspNetCore.Http;
using Mix.Cms.Lib.ViewModels.MixMedias;

namespace Mix.Cms.Api.Controllers
{
    [Route("api/v1/{culture}/media")]
    public class ApiMediaController :
     BaseApiController
    {
        public ApiMediaController()
        {
        }

        #region Get

        // GET api/medias/id
        [HttpGet, HttpOptions]
        [Route("details/{viewType}/{id}")]
        [Route("details/{viewType}")]
        public async Task<JObject> Details(string viewType, int? id = null)
        {
            switch (viewType)
            {
                default:
                    if (id.HasValue)
                    {
                        var feResult = await MediaViewModel.Repository.GetSingleModelAsync(
                        model => model.Id == id && model.Specificulture == _lang).ConfigureAwait(false);
                        return JObject.FromObject(feResult);
                    }
                    else
                    {
                        var media = new MixMedia()
                        {
                            Specificulture = _lang,
                            Priority = MediaViewModel.Repository.Max(a => a.Priority).Data + 1
                        };
                        var result = new RepositoryResponse<MediaViewModel>()
                        {
                            IsSucceed = true,
                            Data = (await MediaViewModel.InitViewAsync(media))
                        };
                        return JObject.FromObject(result);
                    }

            }
        }
        // GET api/medias/id
        [HttpGet, HttpOptions]
        [Route("clone/{id}")]
        public async Task<JObject> Clone(int id)
        {
            var result = await MediaViewModel.Repository.GetSingleModelAsync(
            model => model.Id == id && model.Specificulture == _lang).ConfigureAwait(false);
            if (result.IsSucceed)
            {
                result.Data.IsClone = true;
                var cloneResult = await result.Data.SaveModelAsync(false);
                return JObject.FromObject(cloneResult);
            }
            else
            {
                return JObject.FromObject(result);
            }
        }

        // GET api/medias/id
        [HttpGet, HttpOptions]
        [Route("delete/{id}")]
        public async Task<RepositoryResponse<MixMedia>> Delete(int id)
        {
            var getMedia = MediaViewModel.Repository.GetSingleModel(a => a.Id == id && a.Specificulture == _lang);
            if (getMedia.IsSucceed)
            {
                return await getMedia.Data.RemoveModelAsync(true).ConfigureAwait(false);
            }
            else
            {
                return new RepositoryResponse<MixMedia>() { IsSucceed = false };
            }
        }

        #endregion Get

        #region Post

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("upload")]
        [HttpPost, HttpOptions]
        public async Task<RepositoryResponse<MediaViewModel>> UploadAsync([FromForm] string fileFolder, [FromForm] string title, [FromForm] string description)
        {
            var files = Request.Form.Files;

            if (files.Count > 0)
            {
                var fileUpload = files.FirstOrDefault();

                string folderPath = CommonHelper.GetFullPath(new[] {
                    MixService.GetConfig<string>("UploadFolder"),
                    fileFolder,
                    DateTime.UtcNow.ToString("MM-yyyy")
                });
                // save web files in wwwRoot
                string uploadPath = CommonHelper.GetFullPath(new[] {
                    MixConstants.Folder.WebRootPath,
                    folderPath
                });

                string fileName =
                CommonHelper.GetFullPath(new[] {
                    "/",
                    await UploadFileAsync(files.FirstOrDefault(), uploadPath).ConfigureAwait(false)
                });
                MediaViewModel media = new MediaViewModel(new MixMedia()
                {
                    Specificulture = _lang,
                    FileName = fileName.Split('.')[0].Substring(fileName.LastIndexOf('/') + 1),
                    FileFolder = folderPath,
                    Extension = fileName.Substring(fileName.LastIndexOf('.')),
                    CreatedDateTime = DateTime.UtcNow,
                    FileType = fileUpload.ContentType.Split('/')[0],
                    FileSize = fileUpload.Length,
                    Title = title ?? fileName.Split('.')[0].Substring(fileName.LastIndexOf('/') + 1),
                    Description = description ?? fileName.Split('.')[0].Substring(fileName.LastIndexOf('/') + 1)
                });
                return await media.SaveModelAsync();
            }
            else
            {
                return new RepositoryResponse<MediaViewModel>();
            }
        }

        // POST api/medias
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost, HttpOptions]
        [Route("save")]
        public async Task<RepositoryResponse<MediaViewModel>> Post([FromBody]MediaViewModel model)
        {
            if (model != null)
            {
                model.Specificulture = _lang;
                var result = await model.SaveModelAsync(true).ConfigureAwait(false);

                return result;
            }
            return new RepositoryResponse<MediaViewModel>();
        }

        // GET api/medias

        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost, HttpOptions]
        [Route("list")]
        public async Task<RepositoryResponse<PaginationModel<Lib.ViewModels.MixMedias.MediaViewModel>>> GetList([FromBody]RequestPaging request)
        {
            ParseRequestPagingDate(request);
            Expression<Func<MixMedia, bool>> predicate = model =>
                model.Specificulture == _lang
                && (string.IsNullOrWhiteSpace(request.Keyword)
                || (model.FileName.Contains(request.Keyword)
                || model.Title.Contains(request.Keyword)
                || model.Description.Contains(request.Keyword)))
                && (!request.FromDate.HasValue
                    || (model.CreatedDateTime >= request.FromDate.Value)
                )
                && (!request.ToDate.HasValue
                    || (model.CreatedDateTime <= request.ToDate.Value)
                );

            var data = await MediaViewModel.Repository.GetModelListByAsync(predicate, request.OrderBy, request.Direction, request.PageSize, request.PageIndex).ConfigureAwait(false);

            return data;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost, HttpOptions]
        [Route("list/byProduct/{productId}")]
        [Route("list/byProduct")]
        public async Task<RepositoryResponse<PaginationModel<Mix.Cms.Lib.ViewModels.MixProductMedias.ReadViewModel>>> GetListByProduct(RequestPaging request, int? productId = null)
        {
            var data = await Lib.ViewModels.MixProductMedias.ReadViewModel.Repository.GetModelListByAsync(
            m => m.ProductId == productId && m.Specificulture == _lang, request.OrderBy
            , request.Direction, request.PageSize, request.PageIndex)
            .ConfigureAwait(false);

            return data;

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
