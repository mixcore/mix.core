using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mix.Lib.ViewModels;
using System.Linq.Expressions;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mix-page-content")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MixPageContentController
        : MixRestApiControllerBase<MixPageContentViewModel, MixCmsContext, MixPageContent, int>
    {
        public MixPageContentController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            EntityRepository<MixCmsContext, MixCulture, int> cultureRepository,
            MixIdentityService mixIdentityService,
            MixCmsContext context,
            MixCacheService cacheService,
            IQueueService<MessageQueueModel> queueService)
            : base(httpContextAccessor, configuration, mixService, translator, cultureRepository, mixIdentityService, context, queueService)
        {

        }

        #region Overrides


        protected override Expression<Func<MixPageContent, bool>> BuildAndPredicate(SearchRequestDto req)
        {
            var predicate = base.BuildAndPredicate(req);
            predicate = predicate.AndAlsoIf(!string.IsNullOrEmpty(req.Keyword), m => EF.Functions.Like(m.Title.ToLower(), $"%{req.Keyword.ToLower()}%"));
            return predicate;
        }
        #endregion
    }
}
