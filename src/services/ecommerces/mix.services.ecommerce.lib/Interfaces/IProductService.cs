using Mix.Heart.Models;
using Mix.Lib.Models.Common;
using Mix.Services.Ecommerce.Lib.ViewModels;

namespace Mix.Services.Ecommerce.Lib.Interfaces
{
    public interface IProductService
    {
        public Task<PagingResponseModel<ProductViewModel>> SearchPosts(SearchPostQueryModel searchRequest, CancellationToken cancellationToken = default);

        public Task<List<ProductViewModel>> GetRelatedPosts(int postId, CancellationToken cancellationToken = default);
    }
}
