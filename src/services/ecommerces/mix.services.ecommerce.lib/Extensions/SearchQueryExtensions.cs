using Microsoft.EntityFrameworkCore;
using Mix.Database.Entities.Cms;
using Mix.Heart.Entities;
using Mix.Heart.Extensions;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Models.Common;
using Mix.Services.Ecommerce.Lib.Constants;
using Mix.Services.Ecommerce.Lib.Dtos;
using Mix.Services.Ecommerce.Lib.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Services.Ecommerce.Lib.Extensions
{
    public static class SearchQueryExtensions
    {
        public static void ApplyProducFilter<TEntity>(this SearchQueryModel<TEntity, int> searchQuery,
                FilterProductDto dto,
                UnitOfWorkInfo<EcommerceDbContext> ecommerceUOW)
            where TEntity : MixPostContent
        {
            searchQuery.Predicate = searchQuery.Predicate.AndAlso(
                m => m.MixDatabaseName == MixEcommerceConstants.DatabaseNameProductDetails);
            if (dto.Metadata != null)
            {


                Expression<Func<ProductDetails, bool>> productdetailPred =
                    m => m.ParentType == Constant.Enums.MixDatabaseParentType.Post
                        && m.MixTenantId == searchQuery.MixTenantId;
                if (dto.IsFilterMetadata)
                {
                    Expression<Func<ProductDetails, bool>> metadataPred = m => false;
                    metadataPred = metadataPred.OrIf(dto.Metadata.Interior != null, m => m.Metadata.Interior.Intersect(dto.Metadata.Interior).Any());
                    metadataPred = metadataPred.OrIf(dto.Metadata.Lighting != null, m => m.Metadata.Lighting.Intersect(dto.Metadata.Lighting).Any());
                    metadataPred = metadataPred.OrIf(dto.Metadata.Tile != null, m => m.Metadata.Tile.Intersect(dto.Metadata.Tile).Any());
                    metadataPred = metadataPred.OrIf(dto.Metadata.Brands != null, m => m.Metadata.Brands.Intersect(dto.Metadata.Brands).Any());
                    metadataPred = metadataPred.OrIf(dto.Metadata.Decor != null, m => m.Metadata.Decor.Intersect(dto.Metadata.Decor).Any());


                    //productdetailPred = productdetailPred.AndAlso(metadataPred);
                }
                productdetailPred = productdetailPred.AndAlsoIf(dto.MinPrice.HasValue, m => m.Price >= dto.MinPrice);
                productdetailPred = productdetailPred.AndAlsoIf(dto.MaxPrice.HasValue, m => m.Price <= dto.MaxPrice);
                var query = ecommerceUOW.DbContext.ProductDetails
                        .Where(productdetailPred).Select(m => m.ParentId);
                var allowIds = ecommerceUOW.DbContext.ProductDetails.Where(productdetailPred).Select(m => m.ParentId).ToList();
                searchQuery.Predicate = searchQuery.Predicate.AndAlsoIf(allowIds.Count > 0, m => allowIds.Any(p => p == m.Id));
            }
        }
    }
}
