using Microsoft.EntityFrameworkCore;
using mix.services.ecommerce.Domain.Entities;
using mix.services.ecommerce.Domain.ViewModels;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Base;
using Mix.Lib.ViewModels;

namespace mix.services.ecommerce.Domain.Services
{
    public class EcommerceService : TenantServiceBase
    {
        private readonly UnitOfWorkInfo<EcommerceDbContext> _uow;
        public EcommerceService(IHttpContextAccessor httpContextAccessor, UnitOfWorkInfo<EcommerceDbContext> uow) : base(httpContextAccessor)
        {
            _uow = uow;
        }

        public async Task<ShoppingCart?> GetShoppingCartAsync(Guid userId)
        {
            return await _uow.DbContext.ShoppingCart.FirstOrDefaultAsync(m => m.UserId == userId);
        }

        public async Task<ShoppingCartViewModel> CreateNewShoppingCart(MixUserViewModel user)
        {

            var cart = await ShoppingCartViewModel.GetRepository(_uow).GetSingleAsync(m => m.UserId == user.Id);
            
            if (cart != null)
            {
                await cart.DeleteAsync();
            }

            cart = new ShoppingCartViewModel(_uow)
            {
                SysUserDataId = user.UserData.Value<int>("id"),
                UserId = user.Id,
                Title = $"{user.UserName}'s Cart"
            };
            await cart.SaveAsync();
            
            return cart;
        }
    }
}
