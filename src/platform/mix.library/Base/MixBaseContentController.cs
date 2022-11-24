using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Mix.Lib.Services;
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
            MixService mixService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixCmsContext> cmsUow,
            IQueueService<MessageQueueModel> queueService)
            : base(httpContextAccessor, configuration, mixService, translator, mixIdentityService, cmsUow, queueService)
        {
            ContentType = contentType;
            CmsUow = cmsUow;
            UserManager = userManager;
            IdentityService = identityService;
        }

        #region Routes

        [HttpGet("duplicate/{id}")]
        public async Task<ActionResult<TView>> Duplicate(TPrimaryKey id)
        {
            var data = await GetById(id);
            if (data != null)
            {
                data.Id = default;
                data.ParentId = default;
                var newId = await CreateHandlerAsync(data);
                var result = await GetById(newId);
                return Ok(result);
            }
            throw new MixException(MixErrorStatus.NotFound, id);
        }
        #endregion

        #region Overrides
        protected override async Task<PagingResponseModel<TView>> SearchHandler(SearchRequestDto req)
        {
            var result = await base.SearchHandler(req);
            foreach (var item in result.Items)
            {
                await item.LoadContributorsAsync(ContentType, IdentityService);
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
