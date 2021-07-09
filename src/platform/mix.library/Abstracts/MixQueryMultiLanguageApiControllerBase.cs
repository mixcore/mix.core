using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Mix.Heart.Repository;
using Mix.Heart.Entities;
using Mix.Lib.Dtos;
using Mix.Lib.Models.Common;
using Mix.Heart.ViewModel;
using Mix.Shared.Services;
using Mix.Heart.Model;
using Mix.Heart.Helpers;
using Mix.Shared.Constants;
using Mix.Shared.Enums;

namespace Mix.Lib.Abstracts
{
    public abstract class MixQueryMultiLanguageApiControllerBase<TView, TDbContext, TEntity, TPrimaryKey> 
        : MixQueryApiControllerBase<TView, TDbContext, TEntity, TPrimaryKey>
        where TPrimaryKey : IComparable
        where TDbContext : DbContext
        where TEntity : EntityBase<TPrimaryKey>
        where TView : ViewModelBase<TDbContext, TEntity, TPrimaryKey>
    {
        public MixQueryMultiLanguageApiControllerBase(
            MixAppSettingService appSettingService,
            Repository<TDbContext, TEntity, TPrimaryKey> repository): base(appSettingService, repository)
        {
        }

        #region Routes

        [HttpGet("by-culture/{culture}")]
        public virtual async Task<ActionResult<PagingResponseModel<TView>>> GetByCulture(string culture, [FromQuery] SearchRequestDto req)
        {
            var predicate = ReflectionHelper.GetExpression<TEntity>(MixRequestQueryKeywords.Specificulture, culture, Heart.Enums.ExpressionMethod.Eq);
            if (!req.PageSize.HasValue)
            {
                req.PageSize = _appSettingService.GetConfig(MixAppSettingsSection.GlobalSettings, MixAppSettingKeywords.MaxPageSize, _defaultPageSize);
            }
            var searchRequest = new SearchQueryModel<TEntity, TPrimaryKey>(req, predicate);

            return await _repository.GetPagingViewAsync<TView>(searchRequest.Predicate, searchRequest.PagingData);
        }

        #endregion Routes
    }
}
