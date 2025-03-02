using Mix.Heart.Models;
using Mix.Services.Ecommerce.Lib.Dtos;
using Mix.Services.Ecommerce.Lib.ViewModels;
using System.Security.Claims;

namespace Mix.Services.Ecommerce.Lib.Interfaces
{
    public interface IOrderService
    {
        public Task<PagingResponseModel<OrderViewModel>> GetUserOrders(ClaimsPrincipal principal, FilterOrderDto request, CancellationToken cancellationToken = default);

        public Task<OrderViewModel> GetUserOrder(ClaimsPrincipal principal, int orderId, CancellationToken cancellationToken = default);
        public Task<OrderViewModel> GetGuestOrder(int orderId, CancellationToken cancellationToken = default);

        public Task CancelOrder(ClaimsPrincipal principal, int id, CancellationToken cancellationToken = default);
    }
}
