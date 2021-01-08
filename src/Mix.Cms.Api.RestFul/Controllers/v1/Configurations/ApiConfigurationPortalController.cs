﻿// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Mvc;
using Mix.Cms.Lib;
using Mix.Cms.Lib.Controllers;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.ViewModels.MixConfigurations;
using Mix.Domain.Core.ViewModels;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Constants;
namespace Mix.Cms.Api.RestFul.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1/rest/{culture}/configuration")]
    public class ApiConfigurationPortalController :
        BaseRestApiController<MixCmsContext, MixConfiguration, UpdateViewModel, ReadMvcViewModel, UpdateViewModel>
    {

        // GET: api/s
        [HttpGet]
        public override async Task<ActionResult<PaginationModel<ReadMvcViewModel>>> Get()
        {
            bool isStatus = Enum.TryParse(Request.Query["status"], out MixContentStatus status);
            bool isFromDate = DateTime.TryParse(Request.Query["fromDate"], out DateTime fromDate);
            bool isToDate = DateTime.TryParse(Request.Query["toDate"], out DateTime toDate);
            string keyword = Request.Query["keyword"];
            string category = Request.Query["category"];
            Expression<Func<MixConfiguration, bool>> predicate = model =>
                model.Specificulture == _lang
                && (!isStatus || model.Status == status)
                && (!isFromDate || model.CreatedDateTime >= fromDate)
                && (!isToDate || model.CreatedDateTime <= toDate)
                && (string.IsNullOrEmpty(keyword)
                 || model.Keyword.Contains(keyword)
                 || model.Value.Contains(keyword)
                 )
                 && (string.IsNullOrEmpty(category)
                 || model.Category == category
                 );
            var getData = await base.GetListAsync<ReadMvcViewModel>(predicate);
            if (getData.IsSucceed)
            {
                return getData.Data;
            }
            else
            {
                return BadRequest(getData.Errors);
            }
        }
    }

}