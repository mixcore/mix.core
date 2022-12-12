using Microsoft.AspNetCore.Http;
using Mix.Database.Entities.Cms;
using Mix.Heart.Models;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Models.Common;
using Mix.Services.Databases.Lib.Abtracts;
using Mix.Services.Databases.Lib.Services;
using Mix.Services.Ecommerce.Lib.Entities.Mix;
using Mix.Services.Ecommerce.Lib.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Services.Ecommerce.Lib.Services
{
    public sealed class ProductService : MixPostServiceBase<ProductViewModel>
    {
        private readonly UnitOfWorkInfo<EcommerceDbContext> _ecommerceUOW;
        public ProductService(UnitOfWorkInfo<MixCmsContext> uow, MixMetadataService metadataService, IHttpContextAccessor httpContextAccessor, UnitOfWorkInfo<EcommerceDbContext> ecommerceDbContext) : base(uow, metadataService, httpContextAccessor)
        {
            _ecommerceUOW = ecommerceDbContext;
        }

        public override async Task<PagingResponseModel<ProductViewModel>> SearchPosts(SearchPostQueryModel searchRequest, CancellationToken cancellationToken = default)
        {
            var result = await base.SearchPosts(searchRequest, cancellationToken);
            foreach (var item in result.Items)
            {
                await item.LoadAdditionalDataAsync(_ecommerceUOW, _metadataService, cancellationToken);
            }
            return result;
        }

        public override async Task<List<ProductViewModel>> GetRelatedPosts(int postId, CancellationToken cancellationToken = default)
        {
            var result = await base.GetRelatedPosts(postId, cancellationToken);
            foreach (var item in result)
            {
                await item.LoadAdditionalDataAsync(_ecommerceUOW, _metadataService, cancellationToken);
            }
            return result;
        }
    }
}
