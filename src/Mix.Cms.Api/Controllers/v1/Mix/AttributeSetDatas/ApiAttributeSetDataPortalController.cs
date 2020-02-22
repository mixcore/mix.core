// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Cms.Lib.ViewModels;
using Mix.Cms.Lib.ViewModels.MixAttributeSetDatas;
using Mix.Domain.Core.ViewModels;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using static Mix.Cms.Lib.MixEnums;

namespace Mix.Cms.Api.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1/{culture}/attribute-set-data/portal")]
    public class ApiAttributeSetDataPortalController :
        BaseGenericApiController<MixCmsContext, MixAttributeSetData>
    {
        public ApiAttributeSetDataPortalController(MixCmsContext context, IMemoryCache memoryCache, Microsoft.AspNetCore.SignalR.IHubContext<Hub.PortalHub> hubContext) : base(context, memoryCache, hubContext)
        {
        }

        #region Get

        // GET api/attribute-set-data/id
        [HttpGet, HttpOptions]
        [Route("sendmail/{id}")]
        public Task<RepositoryResponse<JArray>> SendMailAsync(string id)
        {
            return SendMailListAsync(model => model.Id == id && model.Specificulture == _lang);
        }

        // GET api/attribute-set-data/id
        [HttpGet, HttpOptions]
        [Route("delete/{id}")]
        public async Task<RepositoryResponse<MixAttributeSetData>> DeleteAsync(string id)
        {
            return await base.DeleteAsync<DeleteViewModel>(model => model.Id == id, true);
        }

        // GET api/attribute-set-datas/id
        [HttpGet, HttpOptions]
        [Route("details/{id}/{viewType}")]
        [Route("details/{viewType}")]
        public async Task<ActionResult<JObject>> Details(string viewType, string id)
        {
            string msg = string.Empty;
            switch (viewType)
            {
                case "portal":
                    if (!string.IsNullOrEmpty(id))
                    {
                        Expression<Func<MixAttributeSetData, bool>> predicate = model => model.Id == id;
                        var portalResult = await base.GetSingleAsync<MobileViewModel>($"{viewType}_{id}", predicate);
                        return Ok(JObject.FromObject(portalResult));
                    }
                    else
                    {
                        var model = new MixAttributeSetData()
                        {
                            Status = MixService.GetConfig<int>("DefaultStatus")
                            ,
                            Priority = MobileViewModel.Repository.Max(a => a.Priority).Data + 1
                        };

                        RepositoryResponse<MobileViewModel> result = await base.GetSingleAsync<MobileViewModel>($"{viewType}_default", null, model);
                        return Ok(JObject.FromObject(result));
                    }
                default:
                    if (!string.IsNullOrEmpty(id))
                    {
                        Expression<Func<MixAttributeSetData, bool>> predicate = model => model.Id == id;
                        var result = await base.GetSingleAsync<ReadViewModel>($"{viewType}_{id}", predicate);
                        return Ok(JObject.FromObject(result));
                    }
                    else
                    {
                        var model = new MixAttributeSetData()
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

        // POST api/attribute-set-data
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin, Admin")]
        [HttpPost, HttpOptions]
        [Route("save")]
        public async Task<RepositoryResponse<MobileViewModel>> Save([FromBody]MobileViewModel data)
        {
            if (data != null)
            {
                data.Specificulture = _lang;
                var result = await base.SaveAsync<MobileViewModel>(data, true);
                if (result.IsSucceed)
                {
                    MixService.LoadFromDatabase();
                    MixService.SaveSettings();
                }
                return result;
            }
            return new RepositoryResponse<MobileViewModel>() { Status = 501 };
        }

        // GET api/attribute-set-data
        [HttpPost, HttpOptions]
        [Route("list")]
        public async Task<ActionResult<JObject>> GetList(
            [FromBody] RequestPaging request)
        {
            var queryDictionary = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(request.Query);
            var queries = HttpUtility.ParseQueryString(request.Query ?? "");
            int.TryParse(queries.Get("attributeSetId"), out int attributeSetId);
            string attributeSetName = queries.Get("attributeSetName");
            ParseRequestPagingDate(request);

            switch (request.Key)
            {
                case "portal":
                    if (!string.IsNullOrEmpty(request.Query))
                    {
                        var portalResult = await Helper.FilterByKeywordAsync<MobileViewModel>(_lang, attributeSetName,
                        request, request.Keyword, queryDictionary);
                        return Ok(JObject.FromObject(portalResult));
                    }
                    else
                    {
                        Expression<Func<MixAttributeSetData, bool>> predicate = m => (m.AttributeSetId == attributeSetId || m.AttributeSetName == attributeSetName) && m.Specificulture == _lang;
                        var portalResult = await base.GetListAsync<UpdateViewModel>(request, predicate);
                        return Ok(JObject.FromObject(portalResult));
                    }
                default:
                    if (!string.IsNullOrEmpty(request.Query))
                    {
                        var portalResult = await Helper.FilterByKeywordAsync<ReadViewModel>(_lang, attributeSetName,
                        request, request.Keyword, queryDictionary);
                        return Ok(JObject.FromObject(portalResult));
                    }
                    else
                    {
                        Expression<Func<MixAttributeSetData, bool>> predicate = m => (m.AttributeSetId == attributeSetId || m.AttributeSetName == attributeSetName) && m.Specificulture == _lang;
                        var portalResult = await base.GetListAsync<ReadViewModel>(request, predicate);
                        return Ok(JObject.FromObject(portalResult));
                    }
            }
        }

        // GET api/attribute-set-data
        [HttpPost, HttpOptions]
        [Route("export")]
        public async Task<ActionResult<JObject>> Export(
            [FromBody] RequestPaging request)
        {
            var queryDictionary = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(request.Query);
            var queries = HttpUtility.ParseQueryString(request.Query ?? "");
            int.TryParse(queries.Get("attributeSetId"), out int attributeSetId);
            string attributeSetName = queries.Get("attributeSetName");
            ParseRequestPagingDate(request);
            var data = await Lib.ViewModels.MixAttributeSetDatas.Helper.FilterByKeywordAsync<Lib.ViewModels.MixAttributeSetDatas.ImportViewModel>(_lang, attributeSetName,
                        request, request.Keyword, queryDictionary);
            string exportPath = $"exports/module/{attributeSetName}";
            var jData = new List<JObject>();
            foreach (var item in data.Data.Items)
            {
                jData.Add(item.Data);
            }
            var result = Lib.ViewModels.MixAttributeSetDatas.Helper.ExportAttributeToExcel(jData, string.Empty, exportPath, $"{attributeSetName}", null);
            return Ok(JObject.FromObject(result));
        }

        [HttpPost, HttpOptions]
        [Route("apply-list")]
        public async Task<ActionResult<JObject>> ListActionAsync([FromBody]ListAction<string> data)
        {
            Expression<Func<MixAttributeSetData, bool>> predicate = model =>
                       model.Specificulture == _lang
                       && data.Data.Contains(model.Id);
            var result = new RepositoryResponse<bool>();
            switch (data.Action)
            {
                case "Delete":
                    return Ok(JObject.FromObject(await base.DeleteListAsync<MobileViewModel>(predicate, true)));

                case "SendMail":
                    return Ok(JObject.FromObject(await SendMailListAsync(predicate)));

                case "Export":
                    return Ok(JObject.FromObject(await base.ExportListAsync(predicate, MixStructureType.AttributeSet)));

                default:
                    return JObject.FromObject(new RepositoryResponse<bool>());
            }
        }

        private async Task<RepositoryResponse<JArray>> SendMailListAsync(Expression<Func<MixAttributeSetData, bool>> predicate)
        {
            var data = await MobileViewModel.Repository.GetModelListByAsync(predicate);
            JArray array = new JArray();
            RepositoryResponse<JArray> result = new RepositoryResponse<JArray>()
            {
                IsSucceed = true
            };
            try
            {
                foreach (var item in data.Data)
                {
                    var getAttrSet = await Lib.ViewModels.MixAttributeSets.ReadViewModel.Repository.GetSingleModelAsync(m => m.Name == item.AttributeSetName);

                    if (getAttrSet.IsSucceed)
                    {
                        _ = MixService.SendEdm(_lang, getAttrSet.Data.EdmTemplate, item.Data, getAttrSet.Data.EdmSubject, getAttrSet.Data.EdmFrom);
                    }
                    array.Add(item.Data);
                }
                result.Data = array;
                return result;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
                result.IsSucceed = false;
                return result;
            }
        }

        #endregion Post
    }
}