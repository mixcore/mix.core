using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Mix.Lib.Services;
using Mix.SignalR.Interfaces;
using System.Linq.Expressions;

namespace Mix.Portal.Controllers
{
    [MixAuthorize]
    public abstract class MixBaseContentController<TView, TEntity, TPrimaryKey>
        : MixRestfulApiControllerBase<TView, MixCmsContext, TEntity, TPrimaryKey>
        where TPrimaryKey : IComparable
        where TEntity : MultilingualContentBase<TPrimaryKey>
        where TView : MultilingualContentViewModelBase<MixCmsContext, TEntity, TPrimaryKey, TView>
    {
        protected MixIdentityService IdentityService;
        protected MixContentType ContentType;
        protected TenantUserManager UserManager;
        protected UnitOfWorkInfo<MixCmsContext> CmsUow;

        protected MixBaseContentController(
            MixContentType contentType,
            MixIdentityService identityService,
            TenantUserManager userManager,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixCacheService cacheService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixCmsContext> cmsUow,
            IQueueService<MessageQueueModel> queueService,
            IPortalHubClientService portalHub)
            : base(httpContextAccessor, configuration, cacheService, translator, mixIdentityService, cmsUow, queueService, portalHub)
        {
            ContentType = contentType;
            CmsUow = cmsUow;
            UserManager = userManager;
            IdentityService = identityService;
        }

        #region Routes

        [HttpGet("duplicate/{id}")]
        public async Task<ActionResult<TView>> Duplicate(TPrimaryKey id, CancellationToken cancellationToken = default)
        {
            var data = await GetById(id);
            if (data != null)
            {
                data.Id = default;
                data.ParentId = default;
                var newId = await CreateHandlerAsync(data, cancellationToken);
                var result = await GetById(newId);
                return Ok(result);
            }
            throw new MixException(MixErrorStatus.NotFound, id);
        }
        #endregion

        #region Overrides
        protected override async Task<PagingResponseModel<TView>> SearchHandler(SearchRequestDto req, CancellationToken cancellationToken)
        {
            var result = await base.SearchHandler(req, cancellationToken);
            //foreach (var item in result.Items)
            //{
            //    await item.LoadContributorsAsync(ContentType, IdentityService);
            //}
            return result;
        }
        protected override async Task<TPrimaryKey> CreateHandlerAsync(TView data, CancellationToken cancellationToken = default)
        {
            var result = await base.CreateHandlerAsync(data, cancellationToken);
            await UpdateContributor(result, true, cancellationToken);
            return result;
        }

        protected override async Task UpdateHandler(TPrimaryKey id, TView data, CancellationToken cancellationToken)
        {
            await base.UpdateHandler(id, data, cancellationToken);
            await UpdateContributor(id, false, cancellationToken);
        }

        private async Task UpdateContributor(TPrimaryKey id, bool isCreated, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            Guid.TryParse(UserManager.GetUserId(HttpContext.User), out var userId);
            Expression<Func<MixContributor, bool>> expression = m => m.UserId == userId && m.ContentType == ContentType;
            expression = expression.AndAlsoIf(Guid.TryParse(id.ToString(), out var guidId), m => m.GuidContentId == guidId);
            expression = expression.AndAlsoIf(int.TryParse(id.ToString(), out var integerId), m => m.IntContentId == integerId);

            if (!CmsUow.DbContext.MixContributor.Any(expression))
            {
                await CmsUow.DbContext.MixContributor.AddAsync(new MixContributor()
                {
                    UserId = userId,
                    IsOwner = isCreated,
                    ContentType = ContentType,
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
