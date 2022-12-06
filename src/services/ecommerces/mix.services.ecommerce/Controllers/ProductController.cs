using Microsoft.AspNetCore.Mvc;
using Mix.Database.Entities.Cms;
using Mix.Heart.Models;
using Mix.Heart.Services;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Base;
using Mix.Lib.Services;
using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Mix.RepoDb.Repositories;
using Mix.Service.Services;
using Mix.Services.Ecommerce.Lib.Dtos;
using Mix.Services.Ecommerce.Lib.Entities;
using Mix.Services.Ecommerce.Lib.Extensions;
using Mix.Services.Ecommerce.Lib.ViewModels;

namespace mix.services.ecommerce.Controllers
{
    [Route("api/v2/rest/product")]
    [ApiController]
    public class ProductController : MixQueryApiControllerBase<ProductViewModel, MixCmsContext, MixPostContent, int>
    {
        private readonly UnitOfWorkInfo<EcommerceDbContext> _ecommerceUow;
        protected MixCacheService CacheService;
        public ProductController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixCmsContext> uow,
            UnitOfWorkInfo<EcommerceDbContext> ecommerceUow,
            IQueueService<MessageQueueModel> queueService)
            : base(httpContextAccessor, configuration, mixService, translator, mixIdentityService, uow, queueService)
        {
            _ecommerceUow = ecommerceUow;
            CacheService = new();
        }


        [HttpGet("filter")]
        public async Task<ActionResult<PagingResponseModel<ProductViewModel>>> Filter([FromQuery] FilterProductDto req)
        {
            var searchRequest = BuildSearchRequest(req);
            searchRequest.ApplyProductFilter(req, _ecommerceUow);

            var result = await Repository.GetPagingAsync(searchRequest.Predicate, searchRequest.PagingData);
            return Ok(ParseSearchResult(req, result));
        }

        protected override async Task<ProductViewModel> GetById(int id)
        {
            var result = await base.GetById(id);
            if (result.AdditionalData == null)
            {
                await result.LoadAdditionalDataAsync(_ecommerceUow);
                await CacheService.SetAsync($"{id}/{typeof(ProductViewModel).FullName}", result, typeof(MixPostContent), "full");
            }
            return result;
        }

    }
}
