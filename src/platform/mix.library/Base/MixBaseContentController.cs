using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Mix.Database.Entities.Account;
using Mix.Lib.Services;
using System.Linq.Expressions;

namespace Mix.Portal.Controllers
{
    [MixAuthorize]
    public abstract class MixBaseContentController<TView, TEntity, TPrimaryKey>
        : MixRestApiControllerBase<TView, MixCmsContext, TEntity, TPrimaryKey>
        where TPrimaryKey : IComparable
        where TEntity : MultilingualContentBase<TPrimaryKey>
        where TView : MultilingualContentViewModelBase<MixCmsContext, TEntity, TPrimaryKey, TView>
    {
        protected MixIdentityService _identityService;
        protected MixContentType _contentType;
        protected TenantUserManager _userManager;
        protected MixUser _currentUser;
        protected UnitOfWorkInfo<MixCmsContext> _cmsUOW;
        public MixBaseContentController(
            MixContentType contentType,
            MixIdentityService identityService,
            TenantUserManager userManager,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            EntityRepository<MixCmsContext, MixCulture, int> cultureRepository,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixCacheDbContext> cacheUOW,
            UnitOfWorkInfo<MixCmsContext> cmsUOW,
            IQueueService<MessageQueueModel> queueService)
            : base(httpContextAccessor, configuration, mixService, translator, cultureRepository, mixIdentityService, cacheUOW, cmsUOW, queueService)
        {
            _contentType = contentType;
            _cmsUOW = cmsUOW;
            _userManager = userManager;
            _identityService = identityService;
        }

        #region Overrides
        protected override async Task<PagingResponseModel<TView>> SearchHandler(SearchRequestDto req)
        {
            var result = await base.SearchHandler(req);
            foreach (var item in result.Items)
            {
                await item.LoadContributorsAsync(_contentType, _identityService);
            }
            return result;
        }
        protected override async Task<TPrimaryKey> CreateHandlerAsync(TView data)
        {
            var result = await base.CreateHandlerAsync(data);
            await UpdateContributor(result, true);
            return result;
        }

        protected override async Task UpdateHandler(TPrimaryKey id, TView data)
        {
            await base.UpdateHandler(id, data);
            await UpdateContributor(id, false);
        }

        private async Task UpdateContributor(TPrimaryKey id, bool isCreated)
        {
            Guid.TryParse(_userManager.GetUserId(HttpContext.User), out var userId);
            Expression<Func<MixContributor, bool>> expression = m => m.UserId == userId && m.ContentType == _contentType;
            expression = expression.AndAlsoIf(Guid.TryParse(id.ToString(), out var guidId), m => m.GuidContentId == guidId);
            expression = expression.AndAlsoIf(int.TryParse(id.ToString(), out var integerId), m => m.IntContentId == integerId);

            if (!_cmsUOW.DbContext.MixContributor.Any(expression))
            {
                await _cmsUOW.DbContext.MixContributor.AddAsync(new MixContributor()
                {
                    UserId = userId,
                    IsOwner = isCreated,
                    ContentType = _contentType,
                    GuidContentId = guidId,
                    IntContentId = integerId,
                    CreatedDateTime = DateTime.UtcNow,
                    Status = MixContentStatus.Published
                });
            }
        }
        #endregion
    }
}
