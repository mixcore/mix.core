using Microsoft.AspNetCore.Http;
using Mix.Database.Entities.Cms;
using Mix.Heart.Models;
using Mix.Heart.Services;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Models.Common;
using Mix.Services.Databases.Lib.Abstracts;
using Mix.Services.Databases.Lib.Interfaces;
using Mix.Services.Ecommerce.Lib.Entities.Mix;
using Mix.Services.Ecommerce.Lib.Interfaces;
using Mix.Services.Ecommerce.Lib.ViewModels;

namespace Mix.Services.Ecommerce.Lib.Services
{
    public sealed class ProductService : MixPostServiceBase<ProductViewModel>, IProductService
    {
        private readonly UnitOfWorkInfo<EcommerceDbContext> _ecommerceUow;
        public ProductService(
            UnitOfWorkInfo<MixCmsContext> uow,
            IMixMetadataService metadataService,
            IHttpContextAccessor httpContextAccessor,
            UnitOfWorkInfo<EcommerceDbContext> ecommerceDbContext,
            MixCacheService cacheService)
            : base(uow, metadataService, httpContextAccessor, cacheService)
        {
            _ecommerceUow = ecommerceDbContext;
        }

        public override async Task<PagingResponseModel<ProductViewModel>> SearchPosts(SearchPostQueryModel searchRequest, CancellationToken cancellationToken = default)
        {
            var result = await base.SearchPosts(searchRequest, cancellationToken);
            foreach (var item in result.Items)
            {
                await item.LoadAdditionalDataAsync(_ecommerceUow, MetadataService, CacheService, cancellationToken);
            }
            return result;
        }

        public override async Task<List<ProductViewModel>> GetRelatedPosts(int postId, CancellationToken cancellationToken = default)
        {
            var result = await base.GetRelatedPosts(postId, cancellationToken);
            foreach (var item in result)
            {
                await item.LoadAdditionalDataAsync(_ecommerceUow, MetadataService, CacheService, cancellationToken);
            }
            return result;
        }
    }
}
