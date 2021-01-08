﻿// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Mvc;
using Mix.Cms.Lib;
using Mix.Cms.Lib.Controllers;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.ViewModels.MixRelatedAttributeDatas;
using Mix.Domain.Core.ViewModels;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Constants;
namespace Mix.Cms.Api.RestFul.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1/rest/{culture}/related-attribute-data/portal")]
    public class ApiRelatedAttributeDataPortalController :
        BaseRestApiController<MixCmsContext, MixDatabaseContentAssociation, FormViewModel, FormViewModel, FormViewModel>
    {
        // GET: api/v1/rest/{culture}/related-attribute-data
        [HttpGet]
        public override async Task<ActionResult<PaginationModel<FormViewModel>>> Get()
        {
            bool isStatus = Enum.TryParse(Request.Query["status"], out MixContentStatus status);
            bool isAttributeId = int.TryParse(Request.Query["attributeSetId"], out int attributeSetId);
            bool isFromDate = DateTime.TryParse(Request.Query["fromDate"], out DateTime fromDate);
            bool isToDate = DateTime.TryParse(Request.Query["toDate"], out DateTime toDate);
            bool isParentType = Enum.TryParse(Request.Query["parentType"], out MixDatabaseContentAssociationType parentType);
            string parentId = Request.Query["parentId"];
            string attributeSetName = Request.Query["attributeSetName"];
            Expression<Func<MixDatabaseContentAssociation, bool>> predicate = model =>
                model.Specificulture == _lang
                && (!isStatus || model.Status == status)
                && (!isFromDate || model.CreatedDateTime >= fromDate)
                && (!isToDate || model.CreatedDateTime <= toDate)
                && ((isAttributeId && model.AttributeSetId == attributeSetId) || model.AttributeSetName == attributeSetName)
                && (string.IsNullOrEmpty(parentId)
                 || (model.ParentId == parentId && model.ParentType == parentType)
                 );


            var getData = await base.GetListAsync<FormViewModel>(predicate);
            if (getData.IsSucceed)
            {
                return Ok(getData.Data);
            }
            else
            {
                return BadRequest(getData.Errors);
            }
        }

        [HttpGet]
        [Route("navigation/{name}")]
        public async Task<ActionResult<Lib.ViewModels.MixAttributeSetDatas.Navigation>> GetNavigation(string name)
        {
            var navs = await Lib.ViewModels.MixAttributeSetDatas.Helper.FilterByKeywordAsync<Lib.ViewModels.MixAttributeSetDatas.NavigationViewModel>(
                _lang, MixDatabaseNames.NAVIGATION, "equal", "name", name);
            return Ok(navs.Data.FirstOrDefault()?.Nav);
        }
    }

}