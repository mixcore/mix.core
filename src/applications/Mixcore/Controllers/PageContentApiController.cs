using Microsoft.AspNetCore.Mvc;
using Mix.Heart.Extensions;
using Mix.Heart.Helpers;
using Mix.Lib.Models.Common;
using Mix.Lib.Services;
using Mix.RepoDb.Repositories;
using Mix.Services.Databases.Lib.Interfaces;

namespace Mixcore.Controllers
{
    [Route("api/v2/rest/mixcore/page-content")]
    public sealed class PageContentApiController : MixQueryApiControllerBase<PageContentViewModel, MixCmsContext, MixPageContent, int>
    {
        private readonly MixRepoDbRepository _mixRepoDbRepository;
        private readonly IMixMetadataService _metadataService;
        public PageContentApiController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixCacheService cacheService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixCmsContext> uow,
            IQueueService<MessageQueueModel> queueService,
            MixRepoDbRepository mixRepoDbRepository,
            IMixMetadataService metadataService)
            : base(httpContextAccessor, configuration, cacheService, translator, mixIdentityService, uow, queueService)
        {
            _mixRepoDbRepository = mixRepoDbRepository;
            _metadataService = metadataService;
        }

        protected override async Task<PageContentViewModel> GetById(int id)
        {
            var result = await base.GetById(id);
            await result.LoadDataAsync(_mixRepoDbRepository, _metadataService, new(), CacheService);
            return result;
        }
    }
}
