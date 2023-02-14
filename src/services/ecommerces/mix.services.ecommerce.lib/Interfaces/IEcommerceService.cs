using Mix.Services.Ecommerce.Lib.Dtos;
using Mix.Services.Ecommerce.Lib.Enums;
using Mix.Services.Ecommerce.Lib.ViewModels;
using Newtonsoft.Json.Linq;
using System.Security.Claims;

namespace Mix.Services.Ecommerce.Lib.Interfaces
{
    public interface IEcommerceService
    {
        public Task<OrderViewModel?> GetShoppingOrder(Guid userId, CancellationToken cancellationToken = default);

        public Task<OrderViewModel> GetOrCreateShoppingOrder(ClaimsPrincipal principal, CancellationToken cancellationToken = default);

        public Task UpdateOrderStatus(int orderId, OrderStatus orderStatus, CancellationToken cancellationToken = default);

        public Task<OrderViewModel> AddToCart(ClaimsPrincipal principal, CartItemDto item, CancellationToken cancellationToken = default);

        public Task<OrderViewModel> UpdateSelectedCartItem(ClaimsPrincipal principal, CartItemDto item, CancellationToken cancellationToken = default);

        public Task<OrderViewModel> RemoveFromCart(ClaimsPrincipal principal, int itemId, CancellationToken cancellationToken = default);

        public Task<string?> Checkout(ClaimsPrincipal principal, PaymentGateway gateway, OrderViewModel checkoutCart, CancellationToken cancellationToken = default);

        public Task<OrderStatus> ProcessPaymentResponse(int orderId, JObject paymentResponse, CancellationToken cancellationToken = default);

        public Task LogAction(int orderId, OrderTrackingAction action, string? note = "");
    }
}
