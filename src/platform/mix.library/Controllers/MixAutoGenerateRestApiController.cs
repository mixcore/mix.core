using Microsoft.EntityFrameworkCore;
using System;
using Mix.Heart.Entities;
using Mix.Heart.Repository;
using Mix.Heart.ViewModel;
using Mix.Shared.Services;
using Mix.Lib.Abstracts;
using Mix.Database.Entities.Cms.v2;
using Microsoft.Extensions.Logging;
using Mix.Lib.Services;
using Mix.Identity.Attributes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Mix.Lib.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [MixAuthorize]
    public class MixAutoGenerateRestApiController<TView, TDbContext, TEntity, TPrimaryKey>
        : MixRestApiControllerBase<TView, TDbContext, TEntity, TPrimaryKey>
        where TPrimaryKey : IComparable
        where TDbContext : DbContext
        where TEntity : EntityBase<TPrimaryKey>
        where TView: ViewModelBase<TDbContext, TEntity, TPrimaryKey>
    {
        public MixAutoGenerateRestApiController(
            ILogger<MixApiControllerBase> logger, 
            MixAppSettingService appSettingService, 
            MixService mixService, 
            TranslatorService translator, 
            Repository<MixCmsContext, MixCulture, int> cultureRepository, 
            Repository<TDbContext, TEntity, TPrimaryKey> repository) 
            : base(logger, appSettingService, mixService, translator, cultureRepository, repository)
        {
        }
    }
}
