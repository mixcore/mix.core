using Microsoft.AspNetCore.Mvc;
using Mix.Database.Entities.Cms;
using Mix.Heart.Models;
using Mix.Heart.Services;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Base;
using Mix.Lib.Models.Common;
using Mix.Lib.Services;
using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Mix.Services.Databases.Lib.Interfaces;
using Mix.Services.Ecommerce.Lib.Entities.Mix;
using Mix.Services.Ecommerce.Lib.Interfaces;
using Mix.Services.Ecommerce.Lib.ViewModels;
using Mix.Shared.Dtos;

namespace Mix.Services.ecommerce.Controllers
{
    [Route("api/v2/rest/ecommerce/product")]
    [ApiController]
    public class ProductController : MixQueryApiControllerBase<ProductViewModel, MixCmsContext, MixPostContent, int>
    {
        private readonly IMixMetadataService _metadataService;
        private readonly IProductService _productService;
        private readonly UnitOfWorkInfo<EcommerceDbContext> _ecommerceUow;
        public ProductController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixCacheService cacheService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixCmsContext> uow,
            UnitOfWorkInfo<EcommerceDbContext> ecommerceUow,
            IQueueService<MessageQueueModel> queueService,
            IMixMetadataService metadataService,
            IProductService productService)
            : base(httpContextAccessor, configuration, cacheService, translator, mixIdentityService, uow, queueService)
        {
            _ecommerceUow = ecommerceUow;
            _metadataService = metadataService;
            _productService = productService;
        }

        protected override async Task<ProductViewModel> GetById(int id)
        {
            var result = await base.GetById(id);
            await result.LoadAdditionalDataAsync(_ecommerceUow, _metadataService, CacheService);
            return result;
        }

        protected override async Task<PagingResponseModel<ProductViewModel>> SearchHandler(SearchRequestDto req, CancellationToken cancellationToken = default)
        {
            var searchPostQuery = new SearchPostQueryModel(Request, req, CurrentTenant.Id);

            var result = await _productService.SearchPosts(searchPostQuery, cancellationToken);
            foreach (var item in result.Items)
            {
                if (item.ProductDetails == null)
                {
                    await item.LoadAdditionalDataAsync(_ecommerceUow, _metadataService, CacheService, cancellationToken);
                }
            }

            return RestApiService.ParseSearchResult(req, result);
        }
    }
}
