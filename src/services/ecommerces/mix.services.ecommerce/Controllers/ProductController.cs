using Microsoft.AspNetCore.Mvc;
using Mix.Database.Entities.Cms;
using Mix.Heart.Extensions;
using Mix.Heart.Helpers;
using Mix.Heart.Models;
using Mix.Heart.Services;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Base;
using Mix.Lib.Services;
using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Mix.RepoDb.Repositories;
using Mix.Service.Services;
using Mix.Services.Ecommerce.Lib.Constants;
using Mix.Services.Ecommerce.Lib.Dtos;
using Mix.Services.Ecommerce.Lib.Entities;
using Mix.Services.Ecommerce.Lib.Extensions;
using Mix.Services.Ecommerce.Lib.ViewModels;
using Mix.Shared.Dtos;

namespace mix.services.ecommerce.Controllers
{
    [Route("api/v2/rest/product")]
    [ApiController]
    public class ProductController : MixQueryApiControllerBase<ProductViewModel, MixCmsContext, MixPostContent, int>
    {
        private readonly UnitOfWorkInfo<EcommerceDbContext> _ecommerceUOW;
        private readonly MixRepoDbRepository _mixRepoDbRepository;
        protected MixCacheService _cacheService;
        public ProductController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixCmsContext> uow,
            UnitOfWorkInfo<EcommerceDbContext> ecommerceUOW,
            IQueueService<MessageQueueModel> queueService,
            MixRepoDbRepository mixRepoDbRepository)
            : base(httpContextAccessor, configuration, mixService, translator, mixIdentityService, uow, queueService)
        {
            _mixRepoDbRepository = mixRepoDbRepository;
            _ecommerceUOW = ecommerceUOW;
            _cacheService = new();
        }


        [HttpGet("filter")]
        public async Task<ActionResult<PagingResponseModel<ProductViewModel>>> Filter([FromQuery] FilterProductDto req)
        {
            var searchRequest = BuildSearchRequest(req);
            searchRequest.ApplyProducFilter(req, _ecommerceUOW);

            var result = await Repository.GetPagingAsync(searchRequest.Predicate, searchRequest.PagingData);
            return Ok(ParseSearchResult(req, result));
        }

        protected override async Task<ProductViewModel> GetById(int id)
        {
            var result = await base.GetById(id);
            if (result.AdditionalData == null)
            {
                await result.LoadAdditionalDataAsync(_ecommerceUOW);
                await _cacheService.SetAsync($"{id}/{typeof(ProductViewModel).FullName}", result, typeof(MixPostContent), "full");
            }
            return result;
        }

    }
}
