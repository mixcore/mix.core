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
using System.Reflection;
using Mix.Heart.Model;
using Mix.Shared.Enums;
using Mix.Shared.Constants;

namespace Mix.Lib.Abstracts
{
    public class MixQueryApiControllerBase<TView, TDbContext, TEntity, TPrimaryKey> : ControllerBase
        where TPrimaryKey : IComparable
        where TDbContext : DbContext
        where TEntity : EntityBase<TPrimaryKey>
        where TView : ViewModelBase<TDbContext, TEntity, TPrimaryKey>
    {
        protected readonly Repository<TDbContext, TEntity, TPrimaryKey> _repository;
        protected readonly MixAppSettingService _appSettingService;
        protected const int _defaultPageSize = 1000;
        protected string _lang;
        protected bool _forbidden;
        protected string _domain;
        protected ConstructorInfo classConstructor = typeof(TView).GetConstructor(new Type[] { typeof(TEntity) });

        public MixQueryApiControllerBase(
            MixAppSettingService appSettingService,
            Repository<TDbContext, TEntity, TPrimaryKey> repository)
        {
            _repository = repository;
            _appSettingService = appSettingService;
        }

        #region Routes

        [HttpGet]
        public virtual async Task<ActionResult<PagingResponseModel<TView>>> Get([FromQuery] SearchRequestDto req)
        {
            if (!req.PageSize.HasValue)
            {
                req.PageSize = _appSettingService.GetConfig(MixAppSettingsSection.GlobalSettings, MixAppSettingKeywords.MaxPageSize, _defaultPageSize);
            }
            var searchRequest = new SearchQueryModel<TEntity, TPrimaryKey>(req);

            return await _repository.GetPagingViewAsync<TView>(searchRequest.Predicate, searchRequest.PagingData);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TView>> GetSingle(TPrimaryKey id)
        {
            var getData = await _repository.GetSingleViewAsync<TView>(id);
            return Ok(getData);
        }

        #endregion Routes
    }
}
