// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.SignalR.Hubs;
using Mix.Cms.Lib.ViewModels;
using Mix.Heart.Models;
using Mix.Infrastructure.Repositories;
using Mix.Services;

namespace Mix.Cms.Api.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1/file")]
    public class ApiFileController : BaseApiController<MixCmsContext>
    {
        public ApiFileController(MixCmsContext context, IMemoryCache memoryCache, Microsoft.AspNetCore.SignalR.IHubContext<PortalHub> hubContext) : base(context, memoryCache, hubContext)
        {
        }

        #region Post

        // Post api/files/id
        [HttpGet, HttpOptions]
        [Route("details")]
        public RepositoryResponse<FileViewModel> Details(string folder, string filename)
        {
            // Request: Key => folder, Keyword => filename
            if (!string.IsNullOrEmpty(folder))
            {
                var file = MixFileRepository.Instance.GetFile(filename, folder);

                return new RepositoryResponse<FileViewModel>()
                {
                    IsSucceed = file != null,
                    Data = file
                };
            }
            else
            {
                return new RepositoryResponse<FileViewModel>();
            }
        }

        // GET api/files/id
        [HttpGet, HttpOptions]
        [Route("delete")]
        public RepositoryResponse<bool> Delete()
        {
            string fullPath = Request.Query["fullPath"].ToString();
            var result = MixFileRepository.Instance.DeleteFile(fullPath);
            return new RepositoryResponse<bool>()
            {
                IsSucceed = result,
                Data = result
            };
        }

        // POST api/values
        /// <summary>
        /// Uploads the image.
        /// </summary>
        /// <param name="image">The img information.</param>
        /// <param name="file"></param> Ex: { "base64": "", "fileFolder":"" }
        /// <returns></returns>
        [Route("upload-file")]
        [HttpPost, HttpOptions]
        public IActionResult Upload([FromForm] string folder, [FromForm] IFormFile file)
        {
            if (ModelState.IsValid)
            {
                var result = MixFileRepository.Instance.SaveFile(file, $"{folder}");
                return Ok(result);
            }
            return BadRequest();
        }

        // POST api/files
        [HttpPost, HttpOptions]
        [Route("save")]
        public RepositoryResponse<FileViewModel> Save([FromBody] FileViewModel model)
        {
            if (model != null)
            {
                var result = MixFileRepository.Instance.SaveFile(model);
                return new RepositoryResponse<FileViewModel>()
                {
                    IsSucceed = result,
                    Data = model
                };
            }
            return new RepositoryResponse<FileViewModel>();
        }

        // GET api/files
        [HttpPost, HttpOptions]
        [Route("list")]
        public RepositoryResponse<FilePageViewModel> GetList([FromBody] RequestPaging request)
        {
            ParseRequestPagingDate(request);
            if (!request.Key.StartsWith(MixFolders.WebRootPath))
            {
                return new();
            }
            var files = MixFileRepository.Instance.GetTopFiles(request.Key);
            var directories = MixFileRepository.Instance.GetTopDirectories(request.Key);
            return new RepositoryResponse<FilePageViewModel>()
            {
                IsSucceed = true,
                Data = new FilePageViewModel()
                {
                    Files = files,
                    Directories = directories
                }
            };
        }

        #endregion Post
    }
}