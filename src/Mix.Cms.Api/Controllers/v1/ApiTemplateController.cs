// Licensed to the Mix I/O Foundation under one or more agreements.
// The Mix I/O Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mix.Domain.Core.ViewModels;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using static Mix.Cms.Lib.MixEnums;
using System.Linq.Expressions;

namespace Mix.Cms.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "SuperAdmin, Admin")]
    [Produces("application/json")]
    [Route("api/v1/{culture}/template")]
    public class ApiTemplateController :
            BaseApiController
    {

        public ApiTemplateController(Microsoft.AspNetCore.SignalR.IHubContext<Hub.PortalHub> hubContext) : base(hubContext)
        {
        }
        #region Get

        [HttpGet, HttpOptions]
        [Route("details/{viewType}/{themeId}/{folderType}/{id}")]
        [Route("details/{viewType}/{themeId}/{folderType}")]
        public async Task<RepositoryResponse<Lib.ViewModels.MixTemplates.UpdateViewModel>> DetailsAsync(string viewType, int themeId, string folderType, int? id)
        {
            if (id.HasValue)
            {
                var beResult = await Lib.ViewModels.MixTemplates.UpdateViewModel.Repository.GetSingleModelAsync(
                    model => model.Id == id && model.ThemeId == themeId).ConfigureAwait(false);
                return beResult;
            }
            else
            {
                var getTheme = await Lib.ViewModels.MixThemes.ReadViewModel.Repository.GetSingleModelAsync(t => t.Id == themeId);
                if (getTheme.IsSucceed)
                {
                    var model = new MixTemplate()
                    {
                        Status = (int)MixContentStatus.Preview,
                        ThemeId = themeId,
                        ThemeName = getTheme.Data.Name,
                        Extension = MixService.GetConfig<string>("TemplateExtension"),
                        FolderType = folderType
                    };

                    RepositoryResponse<Lib.ViewModels.MixTemplates.UpdateViewModel> result = new RepositoryResponse<Lib.ViewModels.MixTemplates.UpdateViewModel>()
                    {
                        IsSucceed = true,
                        Data = await Lib.ViewModels.MixTemplates.UpdateViewModel.InitViewAsync(model)
                    };
                    result.Data.Specificulture = _lang;
                    return result;
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
            var getPage = await Lib.ViewModels.MixTemplates.UpdateViewModel.Repository.GetSingleModelAsync(
                model => model.Id == id);
            if (getPage.IsSucceed)
            {

                return await getPage.Data.RemoveModelAsync(true);
            }
            else
            {
                return new RepositoryResponse<MixTemplate>()
                {
                    IsSucceed = false
                };
            }
        }


        #endregion Get

        #region Post

        // POST api/template
        [HttpPost, HttpOptions]
        [Route("save")]
        public async Task<RepositoryResponse<Lib.ViewModels.MixTemplates.UpdateViewModel>> Save(
            [FromBody] Lib.ViewModels.MixTemplates.UpdateViewModel model)
        {
            if (model != null)
            {
                var result = await model.SaveModelAsync(true).ConfigureAwait(false);
                return result;
            }
            return new RepositoryResponse<Lib.ViewModels.MixTemplates.UpdateViewModel>();
        }

        // POST api/template
        [HttpPost, HttpOptions]
        [Route("save/{id}")]
        public async Task<RepositoryResponse<MixTemplate>> SaveFields(int id, [FromBody]List<EntityField> fields)
        {
            if (fields != null)
            {
                var result = new RepositoryResponse<MixTemplate>() { IsSucceed = true };
                foreach (var property in fields)
                {
                    if (result.IsSucceed)
                    {
                        result = await Lib.ViewModels.MixTemplates.UpdateViewModel.Repository.UpdateFieldsAsync(c => c.Id == id, fields).ConfigureAwait(false);
                    }
                    else
                    {
                        break;
                    }

                }
                return result;
            }
            return new RepositoryResponse<MixTemplate>();
        }

        // GET api/template
        [HttpPost, HttpOptions]
        [Route("list/{themeId}")]
        public async Task<RepositoryResponse<PaginationModel<Lib.ViewModels.MixTemplates.UpdateViewModel>>> GetList(
            int themeId,
            [FromBody]RequestPaging request
            )
        {
            Expression<Func<MixTemplate, bool>> predicate = model =>
                model.ThemeId == themeId
                 && (string.IsNullOrWhiteSpace(request.Key)
                    ||
                    (
                        model.FolderType == (request.Key)
                    ))
                && (string.IsNullOrWhiteSpace(request.Keyword)
                    ||
                    (
                         model.FileName.Contains(request.Keyword)
                        || model.FileFolder.Contains(request.Keyword)
                        || model.FolderType == request.Keyword
                    ));

            var data = await Lib.ViewModels.MixTemplates.UpdateViewModel.Repository.GetModelListByAsync(predicate, request.OrderBy, request.Direction, request.PageSize, request.PageIndex).ConfigureAwait(false);

            return data;
        }

        #endregion Post
    }
}